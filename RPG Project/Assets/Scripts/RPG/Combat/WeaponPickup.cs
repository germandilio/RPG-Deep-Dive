using System;
using UnityEngine;

namespace RPG.Combat
{ 
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField]
        private Weapon weaponType;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            other.GetComponent<Fighter>()?.EquipWeapon(weaponType);
            Destroy(gameObject);
        }
    }
}