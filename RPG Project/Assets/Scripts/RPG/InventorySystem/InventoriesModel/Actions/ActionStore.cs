using System;
using System.Collections.Generic;
using RPG.InventorySystem.InventoriesModel.Inventory;
using SavingSystem;
using UnityEngine;

namespace RPG.InventorySystem.InventoriesModel.Actions
{
    /// <summary>
    /// Provides the storage for an action bar. The bar has a finite number of
    /// slots that can be filled and actions in the slots can be "used".
    /// 
    /// This component should be placed on the GameObject tagged "Player".
    /// </summary>
    public class ActionStore : MonoBehaviour, ISavable
    {
        public event Action OnStoreUpdated;

        // TODO replace with different types of actions and portions
        private readonly Dictionary<int, DockedItemSlot> _dockedItems = new Dictionary<int, DockedItemSlot>();

        private class DockedItemSlot
        {
            public ActionItem item;
            public int number;
        }

        /// <summary>
        /// Get the action at the given index.
        /// </summary>
        public ActionItem GetAction(int index)
        {
            if (_dockedItems.ContainsKey(index))
            {
                return _dockedItems[index].item;
            }

            return null;
        }

        /// <summary>
        /// Get the number of items left at the given index.
        /// </summary>
        /// <returns>
        /// Will return 0 if no item is in the index or the item has
        /// been fully consumed.
        /// </returns>
        public int GetNumber(int index)
        {
            if (_dockedItems.ContainsKey(index))
            {
                return _dockedItems[index].number;
            }

            return 0;
        }

        /// <summary>
        /// Add an item to the given index.
        /// </summary>
        /// <param name="item">What item should be added.</param>
        /// <param name="index">Where should the item be added.</param>
        /// <param name="number">How many items to add.</param>
        public void AddAction(InventoryItem item, int index, int number)
        {
            if (_dockedItems.ContainsKey(index))
            {
                if (ReferenceEquals(item, _dockedItems[index].item))
                    _dockedItems[index].number += number;
            }
            else
            {
                var slot = new DockedItemSlot();
                slot.item = item as ActionItem;
                slot.number = number;
                _dockedItems[index] = slot;
            }

            OnStoreUpdated?.Invoke();
        }

        /// <summary>
        /// Use the item at the given slot. If the item is consumable one
        /// instance will be destroyed until the item is removed completely.
        /// </summary>
        /// <param name="user">The character that wants to use this action.</param>
        /// <returns>False if the action could not be executed.</returns>
        public bool Use(int index, GameObject user)
        {
            if (!_dockedItems.ContainsKey(index)) return false;

            bool isPerformed = _dockedItems[index].item.Use(user);
            if (isPerformed)
            {
                if (_dockedItems[index].item.IsConsumable)
                    RemoveItems(index, 1);   
            }

            return isPerformed;
        }

        /// <summary>
        /// Remove a given number of items from the given slot.
        /// </summary>
        public void RemoveItems(int index, int number)
        {
            if (!_dockedItems.ContainsKey(index)) return;

            _dockedItems[index].number -= number;
            if (_dockedItems[index].number <= 0)
                _dockedItems.Remove(index);

            OnStoreUpdated?.Invoke();
        }

        /// <summary>
        /// What is the maximum number of items allowed in this slot.
        /// </summary>
        /// <returns>Will return int.MaxValue when there is not defined bound.</returns>
        public int MaxAcceptable(InventoryItem item, int index)
        {
            var actionItem = item as ActionItem;
            if (!actionItem) return 0;

            if (_dockedItems.ContainsKey(index) && !ReferenceEquals(item, _dockedItems[index].item))
                return 0;

            if (actionItem.IsConsumable)
                return int.MaxValue;

            if (_dockedItems.ContainsKey(index))
                return 0;

            return 1;
        }

        [Serializable]
        private struct DockedItemRecord
        {
            public string itemID;
            public int number;
        }

        object ISavable.CaptureState()
        {
            var state = new Dictionary<int, DockedItemRecord>();
            foreach (var pair in _dockedItems)
            {
                var record = new DockedItemRecord();
                record.itemID = pair.Value.item.ItemID;
                record.number = pair.Value.number;
                state[pair.Key] = record;
            }

            return state;
        }

        void ISavable.RestoreState(object state)
        {
            var stateDict = (Dictionary<int, DockedItemRecord>) state;
            foreach (var pair in stateDict)
            {
                AddAction(InventoryItem.GetFromID(pair.Value.itemID), pair.Key, pair.Value.number);
            }
        }
    }
}