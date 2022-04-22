using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;

namespace RPG.GameplayCore.Quests.QuestsModel
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "RPG Project/Quests/New Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [Serializable]
        public class Reward
        {
            public int number;
            public InventoryItem item;
            public string displayTitle;

            public string DisplayTitle => displayTitle;
        }

        [Serializable]
        public class ItemReceiving
        {
            [Tooltip("Objective, on which completion player will receive item.")]
            public Objective objective;
            
            public List<Reward> rewards;
        }

        [Serializable]
        public class Objective
        {
            public string reference;
            public string description;
        }
        
        [SerializeField]
        private List<Objective> objectives;

        [SerializeField]
        private List<ItemReceiving> receiving;

        [SerializeField]
        private List<Reward> rewards;

        [SerializeField]
        private string title;

        public string Title => title;
        
        public IReadOnlyList<Objective> Objectives => objectives;

        /// <summary>
        /// Get receiving item by objective reference.
        /// </summary>
        /// <param name="objectiveRef"></param>
        /// <returns>ItemReceiving which contains info about item, number and objective.</returns>
        /// <remarks>Use this method to get ItemReceiving which belongs to objective completion, which reference passed as the parameter.</remarks>
        public List<Reward> GetReceiving(string objectiveRef)
        {
            if (string.IsNullOrEmpty(objectiveRef)) return null;
            
            var item = receiving.Find(item => item.objective.reference == objectiveRef);
            if (item == null) return null;

            return item.rewards;
        }

        public IReadOnlyList<Reward> Rewards => rewards;

        public int ObjectiveCount
        {
            get
            {
                if (objectives == null)
                    return 0;

                return objectives.Count;
            }
        }

        public static Quest GetByName(string questName)
        {
            var quest = Resources.Load<Quest>(questName);
            return quest;
        }
    }
}