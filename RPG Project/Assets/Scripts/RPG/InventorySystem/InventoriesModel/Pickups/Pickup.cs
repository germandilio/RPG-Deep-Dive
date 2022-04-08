using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;

namespace RPG.InventorySystem.InventoriesModel.Pickups
{
    public class Pickup : MonoBehaviour
    {
        private InventoryItem _item;
        private int _number = 1;

        private Inventory.Inventory _inventory;

        public int Number => _number;

        public InventoryItem Item => _item;

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _inventory = player.GetComponent<Inventory.Inventory>();
        }


        /// <summary>
        /// Set the vital data after creating the prefab.
        /// </summary>
        /// <param name="item">The type of item this prefab represents.</param>
        /// <param name="number">The number of items represented.</param>
        public void Setup(InventoryItem item, int number)
        {
            _item = item;
            if (!item.IsStackable)
                number = 1;
            
            _number = number;
        }

        public void PickupItem()
        {
            bool foundSlot = _inventory.AddToFirstEmptySlot(_item, _number);
            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }

        public bool CanBePickedUp()
        {
            return _inventory.HasSpaceFor(_item);
        }
    }
}