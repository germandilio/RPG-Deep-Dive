using System.Collections.Generic;
using System.Linq;
using RPG.GameplayCore.Quests.Rewards;
using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;

namespace RPG.GameplayCore.Quests.QuestsModel
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "RPG Project/Quests/New Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField]
        private string title;
        
        [SerializeField]
        private List<Objective> objectives;

        [SerializeField]
        private List<ObjectiveCompletionReward> receiving;

        [SerializeField]
        private List<QuestCompletionReward> rewards;

        [SerializeField]
        private List<ObjectiveCompletionWithdrawals> withdrawals;
        
        public string Title => title;
        
        public IReadOnlyList<Objective> Objectives => objectives;

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

        internal IEnumerable<QuestCompletionReward> QuestRewards() => rewards;
        
        /// <summary>
        /// Get rewards by objective reference.
        /// </summary>
        /// <param name="objectiveRef">Objective string reference</param>
        /// <returns>List of inventories which contains info about item and number.</returns>
        /// <remarks>Use this method to get rewards list which belongs to objective completion,
        /// which reference passed as the parameter.</remarks>
        internal IEnumerable<InventorySlot> GetRewards(string objectiveRef)
        {
            if (string.IsNullOrEmpty(objectiveRef)) return null;
            
            var item = receiving.Find(item => item.objective.reference == objectiveRef);
            if (item == null) return null;

            return item.rewards;
        }

        /// <summary>
        /// Get withdrawals by objective reference.
        /// </summary>
        /// <param name="objectiveRef">Objective string reference</param>
        /// <returns>List of inventories which contains info about item and number.</returns>
        /// <remarks>Use this method to get list of inventories to steal from player inventory on objective completion
        /// which belongs to objective completion, which reference passed as the parameter.</remarks>
        internal IEnumerable<InventorySlot> GetWithdrawals(string objectiveRef)
        {
            if (string.IsNullOrEmpty(objectiveRef)) return null;
            
            var item = withdrawals.Find(item => item.objective.reference == objectiveRef);
            if (item == null) return null;

            return item.withdrawals;
        }

        /// <summary>
        /// Get rewards on quest completion.
        /// </summary>
        /// <returns>Rewards which contains info about item and number.</returns>
        internal IEnumerable<InventorySlot> GetRewards()
        {
            if (rewards == null || rewards.Count < 1) return null;

            return rewards.Select(item => item.reward);
        }
    }
}