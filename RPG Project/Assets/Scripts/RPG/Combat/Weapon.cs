using Newtonsoft.Json.Schema;
using RPG.Core;
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
        private GameObject projectilePrefab;

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

        private const string WeaponName = "$$$Weapon$$$";

        public bool HasProjectile => projectilePrefab != null;

        public float WeaponRange => weaponRange;
        public float WeaponDamage => weaponDamage;
        public float TimeBetweenAttacks => timeBetweenAttacks;

        public void CreateWeapon(Transform leftHand, Transform rightHand, Animator characterAnimator)
        {
            // unarmed state
            if (weaponPrefab == null) return;

            DestroyOldWeapon(leftHand, rightHand);

            Transform armedHand = DefineHand(leftHand, rightHand);

            GameObject weaponInstance = Instantiate(weaponPrefab, armedHand);
            characterAnimator.runtimeAnimatorController = animatorController;
            weaponInstance.name = WeaponName;
        }

        private void DestroyOldWeapon(Transform left, Transform right)
        {
            Transform oldLeftWeapon = left.Find(WeaponName);
            Transform oldRightWeapon = right.Find(WeaponName);

            if (oldLeftWeapon != null)
            {
                oldLeftWeapon.name = "$$$destroying$$$";
                Destroy(oldLeftWeapon.gameObject);
            }

            if (oldRightWeapon != null)
            {
                oldRightWeapon.name = "$$$destroying$$$";
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

        public void LaunchProjectile(Transform leftHand, Transform rightHand, Health target)
        {
            // TODO: Исправить метоположение лука в руке, чтобы стрелы не ищезали при попадании в самого игрока
            Transform handSpawn = DefineHand(leftHand, rightHand);
            GameObject projectileInstance = Instantiate(projectilePrefab, handSpawn.position, Quaternion.identity);

            projectileInstance.GetComponent<ProjectileController>()?.SetTarget(target, weaponDamage);
        }
    }
}