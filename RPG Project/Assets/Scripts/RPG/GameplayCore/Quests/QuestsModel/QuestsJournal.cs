using System;
using System.Collections.Generic;
using RPG.GameplayCore.Core;
using RPG.InventorySystem.InventoriesModel;
using RPG.InventorySystem.InventoriesModel.Inventory;
using SavingSystem;
using UnityEngine;
using Utils.UI.Hint;

namespace RPG.GameplayCore.Quests.QuestsModel
{
    public class QuestsJournal : MonoBehaviour, IPredicateEvaluator, ISavable
    {
        [Header("User Hint Properties")]
        [SerializeField]
        private string userHintOnItemsReceived = "Получены новые предметы";

        [SerializeField]
        private string userHintWhenInventoryIsFull = "Недостаточно места в инвентаре";
        
        [Header("Quest Journal Properties")]
        [SerializeField]
        private List<QuestStatus> questsStatuses = new List<QuestStatus>();

        public IEnumerable<QuestStatus> QuestsStatuses => questsStatuses;

        public event Action QuestJournalUpdated;

        public void AddQuest(Quest quest)
        {
            if (!Contains(quest))
            {
                QuestStatus newQuestStatus = new QuestStatus(quest);
                questsStatuses.Add(newQuestStatus);
                QuestJournalUpdated?.Invoke();
            }
        }

        public QuestStatus FindQuestStatus(Quest quest)
        {
            return questsStatuses.Find(status => ReferenceEquals(quest, status.Quest));
        }

        public void CompleteQuestObjective(QuestStatus questStatus, string objectiveToComplete)
        {
            if (string.IsNullOrEmpty(objectiveToComplete)) return;
            
            bool success = questStatus.TryMarkCompleted(objectiveToComplete);
            if (success)
            {
                var receiving = questStatus.Quest.GetReceiving(objectiveToComplete);
                GiveRewards(receiving);
                
                // TODO spawn hint
            }

            if (questStatus.Completed)
                GiveRewards(questStatus.Rewards);
            
            QuestJournalUpdated?.Invoke();
        }
        
        public bool? Evaluate(string predicate, string[] parameters)
        {
            if (predicate != "HasQuest" || parameters.Length == 0) return null;

            return Contains(Quest.GetByName(parameters[0]));
        }

        private void GiveRewards(IReadOnlyList<Quest.Reward> rewards)
        {
            if (rewards == null) return;

            var playerInventory = GetComponent<Inventory>();
            var playerDropper = GetComponent<ItemDropper>();

            bool wasDropped = false;
            foreach (var reward in rewards)
            {
                bool success = playerInventory.AddToFirstEmptySlot(reward.item, reward.number);
                if (!success)
                {
                    playerDropper.DropItem(reward.item, reward.number);
                    wasDropped = true;
                }
            }

            HintSpawner.Spawn(wasDropped ? userHintWhenInventoryIsFull : userHintOnItemsReceived);
        }

        private bool Contains(Quest quest)
        {
            return questsStatuses.Find(questStatus => ReferenceEquals(quest, questStatus.Quest)) != null;
        }

        object ISavable.CaptureState()
        {
            var questList = new List<object>();
            foreach (QuestStatus questStatus in questsStatuses)
            {
                questList.Add(questStatus.CaptureState());
            }

            return questList;
        }

        void ISavable.RestoreState(object state)
        {
            var questList = state as List<object>;
            if (questList == null) return;

            questsStatuses.Clear();
            foreach (object quest in questList)
            {
                questsStatuses.Add(new QuestStatus(quest));
            }
        }
    }
}