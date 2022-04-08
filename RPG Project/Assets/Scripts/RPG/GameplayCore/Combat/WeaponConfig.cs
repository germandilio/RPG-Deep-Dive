using RPG.GameplayCore.Attributes;
using RPG.InventorySystem.InventoriesModel.Equipment;
using UnityEngine;

namespace RPG.GameplayCore.Combat
{
    [CreateAssetMenu(fileName = "new weapon", menuName = "RPG Project/Weapons/New Weapon", order = 0)]
    public class WeaponConfig : StatsEquippableItem
    {
        [Header("Weapon Configuration")]
        [Tooltip(
            "Keep it none if this weapon is invisible (ex. fireball weapon have only projectiles, without visible weapon)")]
        [SerializeField]
        private Weapon weaponPrefab;

        [Tooltip("Keep it none if this weapon doesn't have projectiles")]
        [SerializeField]
        private GameObject projectilePrefab;

        [SerializeField]
        private AnimatorOverrideController animatorOverrideController;

        [SerializeField]
        private WeaponType weaponType;

        [Tooltip("Distance to enemy, where player should stop when attacking")]
        [SerializeField]
        private float weaponRange = 2f;

        [Tooltip("Time in seconds between player attacks")]
        [SerializeField]
        private float timeBetweenAttacks = 2f;

        private const string WeaponName = "$$$Current Weapon$$$";
        private const string DestroyingWeaponName = "$$$destroying$$$";

        public bool HasProjectile => projectilePrefab != null;

        public float WeaponRange => weaponRange;

        public float TimeBetweenAttacks => timeBetweenAttacks;

        /// <summary>
        /// Instantiate weapon in hand based on weaponType.
        /// </summary>
        /// <param name="leftHand">Left hand spawn point.</param>
        /// <param name="rightHand">Right hand spawn point.</param>
        /// <param name="characterAnimator">Character overriding animator controller.</param>
        public Weapon CreateWeapon(Transform leftHand, Transform rightHand, Animator characterAnimator)
        {
            DestroyOldWeapon(leftHand, rightHand);
            if (weaponPrefab == null) return null;

            Transform armedHand = DefineHand(leftHand, rightHand);

            Weapon weaponInstance = Instantiate(weaponPrefab, armedHand);
            SetAnimatorController(characterAnimator);

            weaponInstance.gameObject.name = WeaponName;
            return weaponInstance;
        }

        /// <summary>
        /// Overriding animation controller buy animatorOverrideController.
        /// If animatorOverrideController is null (there is no overriding), it will reset controller to default state.
        /// </summary>
        /// <param name="characterAnimator">Character animator.</param>
        private void SetAnimatorController(Animator characterAnimator)
        {
            var overrideController = characterAnimator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverrideController != null)
            {
                // overriding by animatorOverrideController 
                characterAnimator.runtimeAnimatorController = animatorOverrideController;
            }
            // set default state of animator controller if current controller was overriden and want to be overriden by null 
            else if (overrideController != null)
            {
                // parent controller (default state)
                characterAnimator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform left, Transform right)
        {
            Transform oldLeftWeapon = left.Find(WeaponName);
            Transform oldRightWeapon = right.Find(WeaponName);

            if (oldLeftWeapon != null)
            {
                oldLeftWeapon.name = DestroyingWeaponName;
                Destroy(oldLeftWeapon.gameObject);
            }

            if (oldRightWeapon != null)
            {
                oldRightWeapon.name = DestroyingWeaponName;
                Destroy(oldRightWeapon.gameObject);
            }
        }

        private Transform DefineHand(Transform left, Transform right)
        {
            if (weaponType == WeaponType.RightHanded)
                return right;
            if (weaponType == WeaponType.LeftHanded)
                return left;
            return null;
        }

        /// <summary>
        /// Instantiate projectile of type projectilePrefab in hand based on weaponType.
        /// </summary>
        /// <param name="leftHand">Left hand spawn point.</param>
        /// <param name="rightHand">Right hand spawn point.</param>
        /// <param name="target">Target to launch projectile.</param>
        /// <param name="instigator">Who is shooting from this weapon.</param>
        /// <param name="calculatedDamage">Damage to apply for target.</param>
        public void LaunchProjectile(Transform leftHand, Transform rightHand, Health target, GameObject instigator,
            float calculatedDamage)
        {
            if (projectilePrefab == null) return;

            Transform handSpawn = DefineHand(leftHand, rightHand);
            GameObject projectileInstance = Instantiate(projectilePrefab, handSpawn.position, Quaternion.identity);

            projectileInstance.GetComponent<ProjectileController>()?.SetTarget(target, calculatedDamage, instigator);
        }
    }
}