using System;
using System.Runtime.CompilerServices;
using RPG.GameplayCore.Control;
using RPG.InventorySystem.InventoriesModel.Actions;
using RPG.InventorySystem.InventoriesModel.Equipment;
using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.UI.Dragging;

namespace RPG.InventorySystem.UI.Inventories
{
    /// <summary>
    /// Allows the item to be dragged into other slots.
    /// </summary>
    [RequireComponent(typeof(InventoryItemIcon))]
    public class InventoryDragItem : DragItem<InventoryItem>, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private PlayerController _playerController;
        
        protected override void Awake()
        {
            base.Awake();
            var player = GameObject.FindGameObjectWithTag("Player");
            _playerController = player.GetComponent<PlayerController>();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            var icon = gameObject.GetComponent<InventoryItemIcon>();
            icon.CutOffBackground();
            
            ControlRemover.DisablePlayerControl();

            base.OnBeginDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            
            var icon = gameObject.GetComponent<InventoryItemIcon>();
            icon.ShowBackground();
            
            ControlRemover.EnablePlayerControl();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right)
                return;
            
            if (source.GetItem() is EquippableItem equippableItem)
            {
                var slot = Array.Find(FindObjectsOfType<EquipmentSlotUI>(),
                    item => item.EquipLocation == equippableItem.AllowedEquipLocation);

                DropItemIntoContainer(slot);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var item = source.GetItem();
            if (item is EquippableItem)
            {
                _playerController.SetCursor(CursorType.EquippableItem);
            } else if (item is ActionItem)
            {
                _playerController.SetCursor(CursorType.ActionItem);
            } else
            {
                _playerController.SetCursor(CursorType.InventoryItem);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _playerController.SetCursor(CursorType.OnUI);
        }
    }
}