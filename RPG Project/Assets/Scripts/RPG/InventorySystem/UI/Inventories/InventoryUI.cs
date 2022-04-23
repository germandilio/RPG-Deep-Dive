using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;

namespace RPG.InventorySystem.UI.Inventories
{
    /// <summary>
    /// To be placed on the root of the inventory UI. Handles spawning all the
    /// inventory slot prefabs.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        private InventorySlotUI inventoryItemPrefab;

        private Inventory _playerInventory;
        
        private void Awake()
        {
            _playerInventory = Inventory.GetPlayerInventory();
            _playerInventory.InventoryUpdated += Redraw;
        }

        private void Start()
        {
            Redraw();
        }


        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _playerInventory.Size; i++)
            {
                var itemUI = Instantiate(inventoryItemPrefab, transform);
                itemUI.Setup(_playerInventory, i);
            }
        }
    }
}