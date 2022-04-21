using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.GameplayCore.Quests.QuestsModel
{
    public class QuestsJournal : MonoBehaviour
    {
        [SerializeField]
        private List<QuestStatus> questsStatuses = new List<QuestStatus>();

        public IEnumerable<QuestStatus> QuestsStatuses => questsStatuses;

        public event Action QuestJournalUpdated;

        public void AddQuest(Quest quest)
        {
            QuestStatus newQuestStatus = new QuestStatus(quest);
            questsStatuses.Add(newQuestStatus);
            QuestJournalUpdated?.Invoke();
        }
    }
}