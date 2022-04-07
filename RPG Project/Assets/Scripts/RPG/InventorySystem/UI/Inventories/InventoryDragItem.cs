using RPG.GameplayCore.Control;
using RPG.InventorySystem.InventoriesModel;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.UI.Dragging;

namespace RPG.InventorySystem.UI.Inventories
{
    /// <summary>
    /// To be placed on icons representing the item in a slot. Allows the item
    /// to be dragged into other slots.
    /// </summary>
    public class InventoryDragItem : DragItem<InventoryItem>
    {
        public override void OnBeginDrag(PointerEventData eventData)
        {
            ControlRemover.DisablePlayerControl();
            base.OnBeginDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            ControlRemover.EnablePlayerControl();
        }
    }
}