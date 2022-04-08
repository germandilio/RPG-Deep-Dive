using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;

namespace RPG.InventorySystem.InventoriesModel.Actions
{
    /// <summary>
    /// An inventory item that can be placed in the action bar and "Used".
    /// </summary>
    /// <remarks>
    /// Subclasses must override the `Use`
    /// method.
    /// </remarks>
    [CreateAssetMenu(menuName = "RPG Project/Inventory/New ActionItem", fileName = "New ActionItem", order = 3)]
    public class ActionItem : InventoryItem
    {
        [Header("Action Configuration")]
        [Tooltip("Does an instance of this item get consumed every time it's used.")]
        [SerializeField]
        private bool consumable;

        public bool IsConsumable => consumable;


        /// <summary>
        /// Trigger the use of this item.
        /// </summary>
        /// <param name="user">The character that is using this action.</param>
        public virtual void Use(GameObject user)
        {
            Debug.Log("Use subclasses with overriding \'Use\' method instead." + "Using action: " + this);
        }
    }
}