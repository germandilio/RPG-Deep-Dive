using System;
using System.Collections.Generic;
using NaughtyAttributes;
using RPG.GameplayCore.Core;
using RPG.GameplayCore.Core.Conditions;
using RPG.InventorySystem.InventoriesModel;
using RPG.InventorySystem.InventoriesModel.Inventory;
using SavingSystem;
using Unity.Services.Analytics;
using UnityEngine;
using Utils.UI.Hint;

namespace RPG.GameplayCore.Quests.QuestsModel
{
    /// <summary>
    /// Quest journal provides storage for quests.
    /// Contains all information about quests statuses 
    /// </summary>
    public class QuestsJournal : MonoBehaviour, IPredicateEvaluator, ISavable
    {
        [HorizontalLine]
        [Header("User Hint Properties")]
        [SerializeField]
        private string userHintOnItemsReceived = "Получены новые предметы";

        [SerializeField]
        private string userHintWhenInventoryIsFull = "Недостаточно места в инвентаре";

        [SerializeField]
        private string userHintQuestCompleted = "Квест выполнен";

        private readonly List<QuestStatus> _questsStatuses = new List<QuestStatus>();

        public IEnumerable<QuestStatus> QuestsStatuses => _questsStatuses;

        public event Action QuestJournalUpdated;

        public void AddQuest(Quest quest)
        {
            if (!Contains(quest))
            {
                QuestStatus newQuestStatus = new QuestStatus(quest);
                _questsStatuses.Add(newQuestStatus);
                QuestJournalUpdated?.Invoke();
                
                // analytics
                var parameters = new Dictionary<string, object>
                {
                    {"itemName", quest.name}
                };
                
                AnalyticsService.Instance.CustomData("quest_started", parameters);
            }
        }
        
        /// <summary>
        /// Find Quest status by quest.
        /// </summary>
        /// <param name="quest">Quest, which status will be returned.</param>
        public QuestStatus FindQuestStatus(Quest quest)
        {
            return _questsStatuses.Find(status => ReferenceEquals(quest, status.Quest));
        }

        /// <summary>
        /// Mark quest objective as complete.
        /// Give defined rewards and withdrawals.
        /// </summary>
        /// <param name="questStatus">Quest status object to complete.</param>
        /// <param name="objectiveToComplete">Objective reference, which is completed.</param>
        public void CompleteQuestObjective(QuestStatus questStatus, string objectiveToComplete)
        {
            if (string.IsNullOrEmpty(objectiveToComplete)) return;
            
            bool success = questStatus.TryMarkCompleted(objectiveToComplete);
            if (success)
            {
                var receiving = questStatus.RewardsOnObjective(objectiveToComplete);
                GiveRewards(receiving);
                var withdrawals = questStatus.GetWithdrawals(objectiveToComplete);
                WithdrawFromInventory(withdrawals);
            }

            if (questStatus.Completed)
            {
                GiveRewards(questStatus.Rewards());
                HintSpawner.Spawn(userHintQuestCompleted);
                
                // analytics
                var parameters = new Dictionary<string, object>()
                {
                    {"itemName", questStatus.Quest.name}   
                };
                
                AnalyticsService.Instance.CustomData("quest_completed", parameters);
            }
            
            QuestJournalUpdated?.Invoke();
        }

        private void GiveRewards(IEnumerable<InventorySlot> rewards)
        {
            if (rewards == null) return;

            var playerInventory = GetComponent<Inventory>();
            var playerDropper = GetComponent<ItemDropper>();

            bool wasDropped = false;
            foreach (var reward in rewards)
            {
                if (reward.item == null) continue;
                
                bool success = playerInventory.AddToFirstEmptySlot(reward.item, reward.number);
                if (!success)
                {
                    playerDropper.DropItem(reward.item, reward.number);
                    wasDropped = true;
                }
            }

            HintSpawner.Spawn(wasDropped ? userHintWhenInventoryIsFull : userHintOnItemsReceived);
        }

        private void WithdrawFromInventory(IEnumerable<InventorySlot> withdrawals)
        {
            if (withdrawals == null) return;

            var playerInventory = GetComponent<Inventory>();
            foreach (var item in withdrawals)
            {
                bool success = playerInventory.WithdrawItem(item);
                if (!success)
                {
                    Debug.LogError("Try to withdraw items which inventory doesn't contains!" +
                                   $" InventoryItem: {item.item}, amount: {item.number}");
                }
            }
        }

        private bool Contains(Quest quest)
        {
            return _questsStatuses.Find(questStatus => ReferenceEquals(quest, questStatus.Quest)) != null;
        }

        bool? IPredicateEvaluator.Evaluate(PredicateType predicate, string[] parameters)
        {
            if (parameters.Length == 0) return null;

            if (predicate == PredicateType.HasQuest)
                return Contains(Quest.GetByName(parameters[0]));

            if (predicate == PredicateType.CompleteQuest)
            {
                var questStatus = FindQuestStatus(Quest.GetByName(parameters[0]));
                if (questStatus == null) return null;
                return questStatus.Completed;
            }

            return null;
        }

        object ISavable.CaptureState()
        {
            var questList = new List<object>();
            foreach (QuestStatus questStatus in _questsStatuses)
            {
                questList.Add(questStatus.CaptureState());
            }

            return questList;
        }

        void ISavable.RestoreState(object state)
        {
            var questList = state as List<object>;
            if (questList == null) return;

            _questsStatuses.Clear();
            foreach (object quest in questList)
            {
                _questsStatuses.Add(new QuestStatus(quest));
            }
        }
    }
}