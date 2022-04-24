using System;
using NaughtyAttributes;
using RPG.InventorySystem.InventoriesModel.Inventory;

namespace RPG.GameplayCore.Quests.Rewards
{
    [Serializable]
    public class QuestCompletionReward
    {
        [Required]
        public string displayTitle;
        
        [Required]
        public InventorySlot reward;

        public string DisplayTitle => displayTitle;
    }
}