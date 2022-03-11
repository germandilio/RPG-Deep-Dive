using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Collider))]
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField]
        private Weapon weaponType;

        [SerializeField]
        private float seconds = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            other.GetComponent<Fighter>()?.EquipWeapon(weaponType);

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
    }
}