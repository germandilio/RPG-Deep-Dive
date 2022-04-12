using RPG.InventorySystem.InventoriesModel.Inventory;
using SavingSystem;
using UnityEngine;

namespace RPG.InventorySystem.InventoriesModel.Pickups
{
    /// <summary>
    /// Spawns pickups that should exist on first load in a level. This
    /// automatically spawns the correct prefab for a given inventory item.
    /// </summary>
    public class PickupSpawner : MonoBehaviour, ISavable
    {
        [SerializeField]
        private InventoryItem item;

        [SerializeField]
        private int number = 1;

        private void Awake()
        {
            SpawnPickup();
        }


        /// <summary>
        /// Returns the pickup spawned by this class if it exists.
        /// </summary>
        /// <returns>Returns null if the pickup has been collected.</returns>
        public Pickup GetPickup()
        {
            return GetComponentInChildren<Pickup>();
        }

        /// <summary>
        /// True if the pickup was collected.
        /// </summary>
        public bool isCollected()
        {
            return GetPickup() == null;
        }

        private void SpawnPickup()
        {
            var spawnedPickup = item.SpawnPickup(transform.position, number);
            spawnedPickup.transform.SetParent(transform);
        }

        private void DestroyPickup()
        {
            if (GetPickup() != null)
            {
                Destroy(GetPickup().gameObject);
            }
        }

        object ISavable.CaptureState()
        {
            return isCollected();
        }

        void ISavable.RestoreState(object state)
        {
            bool shouldBeCollected = (bool) state;

            if (shouldBeCollected && !isCollected())
            {
                DestroyPickup();
            }

            if (!shouldBeCollected && isCollected())
            {
                SpawnPickup();
            }
        }
    }
}