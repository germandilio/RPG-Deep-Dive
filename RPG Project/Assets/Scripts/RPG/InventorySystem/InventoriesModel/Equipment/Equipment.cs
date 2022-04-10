using System;
using System.Collections.Generic;
using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;
using SavingSystem;

namespace RPG.InventorySystem.InventoriesModel.Equipment
{
    /// <summary>
    /// Provides a store for the items equipped to a player. Items are stored by
    /// their equip locations.
    /// 
    /// This component should be placed on the GameObject tagged "Player".
    /// </summary>
    public class Equipment : MonoBehaviour, ISavable
    {
        public event Action OnEquipmentUpdated;

        private readonly Dictionary<EquipLocation, EquippableItem> _equippedItems =
            new Dictionary<EquipLocation, EquippableItem>();

        /// <summary>
        /// Return the item in the given equip location.
        /// </summary>
        public EquippableItem GetItemInSlot(EquipLocation equipLocation)
        {
            if (!_equippedItems.ContainsKey(equipLocation))
            {
                return null;
            }

            return _equippedItems[equipLocation];
        }

        /// <summary>
        /// Add an item to the given equip location. Do not attempt to equip to
        /// an incompatible slot.
        /// </summary>
        public void AddItem(EquipLocation slot, EquippableItem item)
        {
            Debug.Assert(item.AllowedEquipLocation == slot);

            _equippedItems[slot] = item;

            OnEquipmentUpdated?.Invoke();
        }

        /// <summary>
        /// Remove the item for the given slot.
        /// </summary>
        public void RemoveItem(EquipLocation slot)
        {
            _equippedItems.Remove(slot);
            OnEquipmentUpdated?.Invoke();
        }

        /// <summary>
        /// Enumerate through all the slots that currently contain items.
        /// </summary>
        public IEnumerable<EquipLocation> GetAllEquipLocations()
        {
            return _equippedItems.Keys;
        }


        object ISavable.CaptureState()
        {
            var equippedItemsForSerialization = new Dictionary<EquipLocation, string>();
            foreach (var pair in _equippedItems)
            {
                equippedItemsForSerialization[pair.Key] = pair.Value.ItemID;
            }

            return equippedItemsForSerialization;
        }

        void ISavable.RestoreState(object state)
        {
            _equippedItems.Clear();
            var equippedItemsForSerialization = (Dictionary<EquipLocation, string>) state;

            foreach (var pair in equippedItemsForSerialization)
            {
                var item = InventoryItem.GetFromID(pair.Value) as EquippableItem;
                if (item != null)
                {
                    _equippedItems[pair.Key] = item;
                }
            }
            
            OnEquipmentUpdated?.Invoke();
        }
    }
}