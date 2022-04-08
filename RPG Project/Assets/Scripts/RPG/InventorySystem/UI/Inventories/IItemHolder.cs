using RPG.InventorySystem.InventoriesModel;
using RPG.InventorySystem.InventoriesModel.Inventory;

namespace RPG.InventorySystem.UI.Inventories
{
    /// <summary>
    /// Allows the `ItemTooltipSpawner` to display the right information.
    /// </summary>
    public interface IItemHolder
    {
        InventoryItem GetItem();
    }
}