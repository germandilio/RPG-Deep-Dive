using RPG.GameplayCore.Control;
using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine.EventSystems;
using Utils.UI.Dragging;

namespace RPG.InventorySystem.UI.Inventories
{
    /// <summary>
    /// Allows the item to be dragged into other slots.
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