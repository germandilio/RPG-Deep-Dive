using System;
using RPG.GameplayCore.Quests.QuestsModel;
using TMPro;
using UnityEngine;

namespace RPG.GameplayCore.Quests.UI
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private TextMeshProUGUI progress;

        private QuestStatus _quest;

        public event Action<QuestStatus> OpeningQuest;

        /// <summary>
        /// Set up quest UI based on quest status info.
        /// </summary>
        /// <param name="quest">Quest status object, which contains quest and completion status info.</param>
        public void Setup(QuestStatus quest)
        {
            _quest = quest;

            title.text = quest.Quest.Title;
            progress.text = $"{quest.CompletedCount}/{quest.Quest.ObjectiveCount}";
        }

        public void ShowDescription()
        {
            OpeningQuest?.Invoke(_quest);
        }
    }
}