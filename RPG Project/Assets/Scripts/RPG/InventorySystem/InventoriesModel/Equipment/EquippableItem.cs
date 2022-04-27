using NaughtyAttributes;
using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;

namespace RPG.InventorySystem.InventoriesModel.Equipment
{
    /// <summary>
    /// An inventory item that can be equipped to the player. Weapons could be a
    /// subclass of this.
    /// </summary>
    [CreateAssetMenu(menuName = "RPG Project/Inventory/New Equippable Item", fileName = "New EquippableItem", order = 2)]
    public class EquippableItem : InventoryItem
    {
        [HorizontalLine]
        [Header("Equipment Location")]
        [InspectorName("Allowed Equipment Slot")]
        [Tooltip("Where are we allowed to put this item.")]
        [SerializeField]
        private EquipLocation allowedEquipLocation;

        public EquipLocation AllowedEquipLocation => allowedEquipLocation;
    }
}