using RPG.InventorySystem.InventoriesModel;
using RPG.InventorySystem.InventoriesModel.Equipment;
using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;
using Utils.UI.Dragging;

namespace RPG.InventorySystem.UI.Inventories
{
    /// <summary>
    /// An slot for the players equipment.
    /// </summary>
    public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField]
        private InventoryItemIcon icon;

        [SerializeField]
        private EquipLocation equipLocation = EquipLocation.Weapon;

        private Equipment _playerEquipment;


        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _playerEquipment = player.GetComponent<Equipment>();
            _playerEquipment.EquipmentUpdated += OnInventoryUpdated;
        }

        private void Start()
        {
            OnInventoryUpdated();
        }


        public int MaxAcceptable(InventoryItem item)
        {
            EquippableItem equippableItem = item as EquippableItem;
            if (equippableItem == null) return 0;
            if (equippableItem.AllowedEquipLocation != equipLocation) return 0;
            if (GetItem() != null) return 0;

            return 1;
        }

        public void AddItems(InventoryItem item, int number)
        {
            _playerEquipment.AddItem(equipLocation, (EquippableItem) item);
        }

        public InventoryItem GetItem()
        {
            return _playerEquipment.GetItemInSlot(equipLocation);
        }

        public int GetNumber()
        {
            if (GetItem() != null)
                return 1;

            return 0;
        }

        public void RemoveItems(int number)
        {
            _playerEquipment.RemoveItem(equipLocation);
        }


        protected virtual void OnInventoryUpdated()
        {
            icon.SetItem(_playerEquipment.GetItemInSlot(equipLocation));
        }
    }
}