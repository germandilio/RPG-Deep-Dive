using System;
using RPG.InventorySystem.InventoriesModel.Inventory;

namespace RPG.GameplayCore.Quests.Rewards
{
    [Serializable]
    public class QuestCompletionReward
    {
        public string displayTitle;
        public InventorySlot reward;

        public string DisplayTitle => displayTitle;
    }
}