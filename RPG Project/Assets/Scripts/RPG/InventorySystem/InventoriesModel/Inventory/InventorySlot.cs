using System;

namespace RPG.InventorySystem.InventoriesModel.Inventory
{
    [Serializable]
    public class InventorySlot
    {
        public InventoryItem item;
        public int number;
    }
}