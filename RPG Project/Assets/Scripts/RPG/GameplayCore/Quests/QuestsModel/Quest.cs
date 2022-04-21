using System.Collections.Generic;
using UnityEngine;

namespace RPG.GameplayCore.Quests.QuestsModel
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "RPG Project/Quests/New Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField]
        private List<string> objectives;

        [SerializeField]
        private string title;

        [SerializeField]
        private string rewardTitle;

        public string Title => title;

        public string RewardTitle => rewardTitle;

        public IReadOnlyList<string> Objectives => objectives;

        public int ObjectiveCount
        {
            get
            {
                if (objectives == null)
                    return 0;

                return objectives.Count;
            }
        }
    }
}