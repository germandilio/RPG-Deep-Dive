using System;
using RPG.GameplayCore.Core;
using RPG.GameplayCore.Core.Conditions;
using UnityEngine;
using SavingSystem;

namespace RPG.InventorySystem.InventoriesModel.Inventory
{
    /// <summary>
    /// Provides storage for the player inventory. A configurable number of
    /// slots are available.
    /// </summary>
    public class Inventory : MonoBehaviour, ISavable, IPredicateEvaluator
    {
        public event Action InventoryUpdated;

        [Header("Inventory Configuration")]
        [Tooltip("Allowed size")]
        [SerializeField]
        private int inventorySize = 16;

        [SerializeField]
        private InventorySlot[] _slots;

        public int Size => _slots.Length;

        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }

        /// <summary>
        /// Attempt to add the items to the first available slot.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="number">The number to add.</param>
        /// <returns>Whether or not the item could be added.</returns>
        public bool AddToFirstEmptySlot(InventoryItem item, int number)
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            _slots[i].item = item;
            _slots[i].number += number;

            InventoryUpdated?.Invoke();
            return true;
        }

        /// <summary>
        /// Is there an instance of the item in the inventory?
        /// </summary>
        public bool HasItem(InventoryItem item)
        {
            if (item == null) return false;
            
            for (int i = 0; i < _slots.Length; i++)
            {
                if (ReferenceEquals(_slots[i].item, item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Return the item type in the given slot.
        /// </summary>
        public InventoryItem GetItemInSlot(int slot)
        {
            return _slots[slot].item;
        }

        /// <summary>
        /// Get the number of items in the given slot.
        /// </summary>
        public int GetNumberInSlot(int slot)
        {
            return _slots[slot].number;
        }

        public void RemoveFromSlot(int slot, int number)
        {
            _slots[slot].number -= number;
            if (_slots[slot].number <= 0)
            {
                _slots[slot].number = 0;
                _slots[slot].item = null;
            }

            InventoryUpdated?.Invoke();
        }

        /// <summary>
        /// Withdraw items in inventory.
        /// </summary>
        /// <param name="slot">Inventory slot which contains info about item and number to withdraw.</param>
        /// <returns>suceeded status</returns>
        public bool WithdrawItem(InventorySlot slot)
        {
            if (slot == null || slot.item == null) return false;

            int slotIndex = FindSlotNonEmpty(slot.item);
            if (slotIndex != -1)
            {
                RemoveFromSlot(slotIndex, slot.number);
                return true;   
            }

            return false;
        }

        /// <summary>
        /// Will add an item to the given slot if possible. If there is already
        /// a stack of this type, it will add to the existing stack. Otherwise,
        /// it will be added to the first empty slot.
        /// </summary>
        /// <param name="slot">The slot to attempt to add to.</param>
        /// <param name="item">The item type to add.</param>
        /// <param name="number">The number of items to add.</param>
        /// <returns>True if the item was added anywhere in the inventory.</returns>
        public bool AddItemToSlot(int slot, InventoryItem item, int number)
        {
            if (_slots[slot].item != null)
            {
                return AddToFirstEmptySlot(item, number);
                ;
            }

            var i = FindStack(item);
            if (i >= 0)
            {
                slot = i;
            }

            _slots[slot].item = item;
            _slots[slot].number += number;
            InventoryUpdated?.Invoke();
            return true;
        }
        
        private void Awake()
        {
            // TODO REPLACE
            Array.Resize(ref _slots, inventorySize);
            for (int i = 0; i < inventorySize; i++)
            {
                _slots[i] ??= new InventorySlot();
            }
        }

        /// <summary>
        /// Find a slot that can accomodate the given item.
        /// </summary>
        /// <returns>-1 if no slot is found.</returns>
        private int FindSlot(InventoryItem item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }

            return i;
        }

        /// <summary>
        /// Find a slot which contains an inventory item.
        /// </summary>
        /// <param name="item">Inventory item to find</param>
        /// <returns>-1 if not contains, otherwise index in inventory.</returns>
        private int FindSlotNonEmpty(InventoryItem item)
        {
            if (_slots.Length < 1) return -1;
            
            int slotIndex;
            for (slotIndex = 0; slotIndex < _slots.Length; slotIndex++)
            {
                if (ReferenceEquals(_slots[slotIndex].item, item))
                    break;
            }

            if (slotIndex == _slots.Length) return -1;
            return slotIndex;
        }

        /// <summary>
        /// Find an empty slot.
        /// </summary>
        /// <returns>-1 if all slots are full.</returns>
        private int FindEmptySlot()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item == null)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Find an existing stack of this item type.
        /// </summary>
        /// <returns>-1 if no stack exists or if the item is not stackable.</returns>
        private int FindStack(InventoryItem item)
        {
            if (!item.IsStackable)
                return -1;

            for (int i = 0; i < _slots.Length; i++)
            {
                if (ReferenceEquals(_slots[i].item, item))
                    return i;
            }

            return -1;
        }

        [Serializable]
        private struct InventorySlotRecord
        {
            public string itemID;
            public int number;
        }

        object ISavable.CaptureState()
        {
            var slotStrings = new InventorySlotRecord[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                if (_slots[i].item != null)
                {
                    slotStrings[i].itemID = _slots[i].item.ItemID;
                    slotStrings[i].number = _slots[i].number;
                }
            }

            return slotStrings;
        }

        void ISavable.RestoreState(object state)
        {
            var slotStrings = (InventorySlotRecord[]) state;
            for (int i = 0; i < inventorySize && i < slotStrings.Length; i++)
            {
                _slots[i].item = InventoryItem.GetFromID(slotStrings[i].itemID);
                _slots[i].number = slotStrings[i].number;
            }

            InventoryUpdated?.Invoke();
        }

        bool? IPredicateEvaluator.Evaluate(PredicateType predicate, string[] parameters)
        {
            if (predicate != PredicateType.HasInventoryItem || parameters.Length < 1) return null;
            return HasItem(InventoryItem.GetFromID(parameters[0]));
        }
    }
}