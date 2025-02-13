﻿using System;
using System.Collections.Generic;
using NaughtyAttributes;
using RPG.InventorySystem.InventoriesModel.Pickups;
using UnityEngine;

namespace RPG.InventorySystem.InventoriesModel.Inventory
{
    /// <summary>
    /// Represents any item that can be put in a inventory.
    /// </summary>
    public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        [ReadOnly]
        [Tooltip("Auto-generated GUID for saving/loading. Clear this field if you want to generate a new one.")]
        [SerializeField]
        private string itemID;

        [HorizontalLine]
        [Header("Tooltip Settings")]
        [Tooltip("Item name to be displayed in UI.")]
        [SerializeField]
        private string displayName;

        [Tooltip("Item description to be displayed in UI.")]
        [TextArea]
        [SerializeField]
        private string description;

        [HorizontalLine]
        [ShowAssetPreview]
        [Header("Inventory Configuration")]
        [Tooltip("The UI icon to represent this item in the inventory (without background).")]
        [SerializeField]
        private Sprite icon;

        [ShowAssetPreview]
        [Tooltip("The UI icon to represent this item in the inventory (with background).")]
        [SerializeField]
        private Sprite iconWithBackground;

        [Tooltip("The prefab that should be spawned when this item is dropped.")]
        [SerializeField]
        private Pickup pickup;

        [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
        [SerializeField]
        private bool stackable;

        private static Dictionary<string, InventoryItem> _itemLookupCache;

        public Sprite Icon => icon;

        public Sprite IconWithBackground => iconWithBackground;

        public string ItemID => itemID;

        public bool IsStackable => stackable;

        public string DisplayName => displayName;

        public string Description => description;

        /// <summary>
        /// Get the inventory item instance from its GUID.
        /// </summary>
        /// <param name="itemID">
        /// String GUID.
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
            var newPickup = Instantiate(this.pickup);
            newPickup.transform.position = position;
            newPickup.Setup(this, number);
            return newPickup;
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