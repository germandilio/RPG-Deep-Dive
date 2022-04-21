using System.Collections.Generic;
using UnityEngine;

namespace RPG.GameplayCore.Quests.QuestsModel
{
    [System.Serializable]
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

        public bool IsObjectiveCompleted(string objective)
        {
            return completedObjectives.Contains(objective);
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }
    }
}