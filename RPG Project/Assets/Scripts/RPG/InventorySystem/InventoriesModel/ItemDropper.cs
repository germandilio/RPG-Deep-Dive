using System.Collections.Generic;
using RPG.InventorySystem.InventoriesModel.Inventory;
using RPG.InventorySystem.InventoriesModel.Pickups;
using UnityEngine;
using SavingSystem;
using UnityEngine.SceneManagement;

namespace RPG.InventorySystem.InventoriesModel
{
    /// <summary>
    /// To be placed on anything that wishes to drop pickups into the world.
    /// Component tracks the drops for saving and restoring.
    /// </summary>
    public class ItemDropper : MonoBehaviour, ISavable
    {
        private readonly List<Pickup> _droppedItems = new List<Pickup>();
        private readonly List<DropRecord> _droppedItemsInOtherScenes = new List<DropRecord>();

        /// <summary>
        /// Create a pickup at the current position.
        /// </summary>
        /// <param name="item">The item type for the pickup.</param>
        /// <param name="number">
        /// The number of items contained in the pickup. Only used if the item
        /// is stackable.
        /// </param>
        public void DropItem(InventoryItem item, int number)
        {
            SpawnPickup(item, GetLocationToDrop(), number);
        }

        /// <summary>
        /// Create a pickup at the current position.
        /// </summary>
        /// <param name="item">The item type for the pickup.</param>
        public void DropItem(InventoryItem item)
        {
            DropItem(item, 1);
        }

        protected virtual Vector3 GetLocationToDrop()
        {
            return transform.position;
        }

        private void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
        {
            var pickup = item.SpawnPickup(spawnLocation, number);
            _droppedItems.Add(pickup);
        }

        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
            public int number;

            public int sceneBuildIndex;
        }

        object ISavable.CaptureState()
        {
            RemoveDestroyedDrops();
            int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

            var droppedItemsList = new List<DropRecord>();
            foreach (var pickup in _droppedItems)
            {
                var droppedItem = new DropRecord
                {
                    itemID = pickup.Item.ItemID,
                    position = new SerializableVector3(pickup.transform.position),
                    number = pickup.Number,
                    sceneBuildIndex = currentBuildIndex
                };
                droppedItemsList.Add(droppedItem);
            }

            droppedItemsList.AddRange(_droppedItemsInOtherScenes);
            return droppedItemsList;
        }

        void ISavable.RestoreState(object state)
        {
            int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

            _droppedItemsInOtherScenes.Clear();
            _droppedItems.Clear();
            
            var droppedItemsList = (List<DropRecord>) state;
            foreach (var item in droppedItemsList)
            {
                if (item.sceneBuildIndex != currentBuildIndex)
                {
                    _droppedItemsInOtherScenes.Add(item);
                }
                else
                {
                    var pickupItem = InventoryItem.GetFromID(item.itemID);
                    Vector3 position = item.position.ToVector();
                    int number = item.number;

                    SpawnPickup(pickupItem, position, number);
                }
            }
        }

        /// <summary>
        /// Remove any drops in the world that have been picked up.
        /// </summary>
        private void RemoveDestroyedDrops()
        {
            _droppedItems.RemoveAll(item => item == null);
        }
    }
}