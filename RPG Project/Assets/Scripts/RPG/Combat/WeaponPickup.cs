using System.Collections;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Collider))]
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField]
        private WeaponConfig weaponConfig;

        [SerializeField]
        private float healthPointsToRestore = 0f;

        [SerializeField]
        private float seconds = 5f;

        //TODO fix the bug with double pickup and moving to pickup
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            PickUp(other.gameObject);
        }

        private void PickUp(GameObject player)
        {
            if (weaponConfig != null)
                player.GetComponent<Fighter>()?.EquipWeapon(weaponConfig);

            if (healthPointsToRestore > 0)
                player.GetComponent<Health>()?.Heal(healthPointsToRestore);

            StartCoroutine(HidePickupForSeconds(seconds));
        }

        private IEnumerator HidePickupForSeconds(float timeInSeconds)
        {
            SetVisibleState(false);
            yield return new WaitForSeconds(timeInSeconds);
            SetVisibleState(true);
        }

        private void SetVisibleState(bool isVisible)
        {
            GetComponent<Collider>().enabled = isVisible;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(isVisible);
            }
        }

        public bool HandleRaycast(PlayerController interactController)
        {
            if (Input.GetMouseButtonDown(0))
                PickUp(interactController.gameObject);
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}