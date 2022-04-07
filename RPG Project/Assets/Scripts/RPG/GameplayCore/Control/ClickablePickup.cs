using System;
using RPG.InventorySystem.InventoriesModel;
using UnityEngine;

namespace RPG.GameplayCore.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        private Pickup _pickup;

        private void Awake()
        {
            _pickup = GetComponent<Pickup>();
        }

        public bool HandleRaycast(PlayerController interactController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _pickup.PickupItem();
            }

            return true;
        }

        public CursorType GetCursorType()
        {
            if (_pickup.CanBePickedUp())
                return CursorType.Pickup;
            
            return CursorType.FullInventory;
        }
    }
}