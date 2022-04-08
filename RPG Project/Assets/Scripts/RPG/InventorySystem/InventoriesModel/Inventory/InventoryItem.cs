using System;
using System.Collections.Generic;
using RPG.InventorySystem.InventoriesModel.Pickups;
using UnityEngine;

namespace RPG.InventorySystem.InventoriesModel.Inventory
{
    /// <summary>
    /// Abstraction that represents any item that can be put in a inventory.
    /// </summary>
    public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("Auto-generated GUID for saving/loading. Clear this field if you want to generate a new one.")]
        [SerializeField]
        private string itemID;

        [Header("Tooltip Settings")]
        [Tooltip("Item name to be displayed in UI.")]
        [SerializeField]
        private string displayName;

        [Tooltip("Item description to be displayed in UI.")]
        [TextArea]
        [SerializeField]
        private string description;

        [Header("Inventory Configuration")]
        [Tooltip("The UI icon to represent this item in the inventory.")]
        [SerializeField]
        private Sprite icon;

        [Tooltip("The prefab that should be spawned when this item is dropped.")]
        [SerializeField]
        private Pickup pickup;

        [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
        [SerializeField]
        private bool stackable;

        private static Dictionary<string, InventoryItem> _itemLookupCache;

        public Sprite Icon => icon;

        public string ItemID => itemID;

        public bool IsStackable => stackable;

        public string DisplayName => displayName;

        public string Description => description;


        /// <summary>
        /// Get the inventory item instance from its GUID.
        /// </summary>
        /// <param name="itemID">
        /// String GUID that persists between game instances.
        /// </param>
        /// <returns>
        /// Inventory item instance corresponding to the ID.
        /// </returns>
        public static InventoryItem GetFromID(string itemID)
        {
            if (_itemLookupCache == null)
                BuildLookup();

            if (itemID == null || !_itemLookupCache.ContainsKey(itemID)) return null;
            return _itemLookupCache[itemID];
        }

        /// <summary>
        /// Spawn the pickup gameObject into the world.
        /// </summary>
        /// <param name="position">Where to spawn the pickup.</param>
        /// <param name="number">How many instances of the item does the pickup represent.</param>
        /// <returns>Reference to the pickup object spawned.</returns>
        public Pickup SpawnPickup(Vector3 position, int number)
        {
            var pickup = Instantiate(this.pickup);
            pickup.transform.position = position;
            pickup.Setup(this, number);
            return pickup;
        }


        private static void BuildLookup()
        {
            _itemLookupCache = new Dictionary<string, InventoryItem>();
            var itemList = Resources.LoadAll<InventoryItem>("");
            foreach (var item in itemList)
            {
                if (_itemLookupCache.ContainsKey(item.itemID))
                {
                    Debug.LogError(
                        $"Looks like there's a duplicate InventorySystem ID for objects: {_itemLookupCache[item.itemID]} and {item}");
                    continue;
                }

                _itemLookupCache[item.itemID] = item;
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // Generate and save a new GUID if this is blank.
            if (string.IsNullOrWhiteSpace(itemID))
            {
                itemID = Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
        }
    }
}