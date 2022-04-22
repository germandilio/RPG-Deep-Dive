using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.GameplayCore.Quests.QuestsModel
{
    [Serializable]
    public class QuestStatus
    {
        [SerializeField]
        private Quest quest;

        [SerializeField]
        private List<string> completedObjectives = new List<string>();

        public Quest Quest => quest;

        public int CompletedCount
        {
            get
            {
                if (completedObjectives == null) return 0;

                return completedObjectives.Count;
            }
        }

        public bool Completed => CompletedCount == quest.ObjectiveCount;
        
        public IReadOnlyList<Quest.Reward> Rewards => quest.Rewards;

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object questSaving)
        {
            var saving = (SavableQuest) questSaving;
            quest = Quest.GetByName(saving.questName);

            completedObjectives = saving.completedObjectives;
        }

        /// <summary>
        /// Mark objective as completed.
        /// </summary>
        /// <param name="completedObjectiveRef">objective reference, which should be marked</param>
        /// <returns>True if successfully completed, false if already completed, ot not exists.</returns>
        public bool TryMarkCompleted(string completedObjectiveRef)
        {
            if (!completedObjectives.Contains(completedObjectiveRef))
            {
                completedObjectives.Add(completedObjectiveRef);
                return true;
            }

            return false;
        }

        [Serializable]
        private class SavableQuest
        {
            public string questName;
            public List<string> completedObjectives;
        } 
        
        public bool IsObjectiveCompleted(string objectiveRef)
        {
            return completedObjectives.Contains(objectiveRef);
        }

        public object CaptureState()
        {
            SavableQuest saving = new SavableQuest
            {
                questName = quest.name,
                completedObjectives = completedObjectives
            };
            
            return saving;
        }
    }
}