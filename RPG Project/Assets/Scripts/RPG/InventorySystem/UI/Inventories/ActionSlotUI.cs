using RPG.InventorySystem.InventoriesModel.Actions;
using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;
using Utils.UI.Dragging;

namespace RPG.InventorySystem.UI.Inventories
{
    /// <summary>
    /// The UI slot for the player action bar.
    /// </summary>
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField]
        private InventoryItemIcon icon;

        [SerializeField]
        private int index;

        private ActionStore _actionStore;


        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _actionStore = player.GetComponent<ActionStore>();
            _actionStore.StoreUpdated += UpdateIcon;
        }


        public void AddItems(InventoryItem item, int number)
        {
            _actionStore.AddAction(item, index, number);
        }

        public InventoryItem GetItem()
        {
            return _actionStore.GetAction(index);
        }

        public int GetNumber()
        {
            return _actionStore.GetNumber(index);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return _actionStore.MaxAcceptable(item, index);
        }

        public void RemoveItems(int number)
        {
            _actionStore.RemoveItems(index, number);
        }


        void UpdateIcon()
        {
            icon.SetItem(GetItem(), GetNumber());
        }
    }
}