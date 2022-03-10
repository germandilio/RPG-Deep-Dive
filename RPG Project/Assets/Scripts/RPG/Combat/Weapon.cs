using Newtonsoft.Json.Schema;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "new weapon", menuName = "Weapons/New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField]
        private GameObject weaponPrefab;
        [SerializeField]
        private AnimatorOverrideController animatorController;

        [SerializeField]
        private WeaponType weaponType;
        
        [SerializeField]
        [Tooltip("Distance to enemy, where player should stop when attacking")]
        private float weaponRange = 2f;

        [SerializeField]
        [Tooltip("Damage, which player apply to Combat target")]
        private float weaponDamage = 25f;

        [SerializeField]
        [Tooltip("Time in seconds between player attacks")]
        private float timeBetweenAttacks = 2f;

        public float WeaponRange => weaponRange;
        public float WeaponDamage => weaponDamage;
        public float TimeBetweenAttacks => timeBetweenAttacks;
        
        public void CreateWeapon(Transform leftHand, Transform rightHand, Animator characterAnimator)
        {
            // unarmed state
            if (weaponPrefab == null) return;

            Transform armedHand = DefineHand(leftHand, rightHand);
            
            Instantiate(weaponPrefab, armedHand);
            characterAnimator.runtimeAnimatorController = animatorController;
        }

        private Transform DefineHand(Transform left, Transform right)
        {
            if (weaponType == WeaponType.RightHanded)
                return right;
            else if (weaponType == WeaponType.LeftHanded)
                return left;
            return null;
        }
    }
}