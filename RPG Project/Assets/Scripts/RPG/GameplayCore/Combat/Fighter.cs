using NaughtyAttributes;
using UnityEngine;
using RPG.GameplayCore.Attributes;
using RPG.GameplayCore.Core;
using RPG.GameplayCore.Movement;
using RPG.GameplayCore.Stats;
using RPG.InventorySystem.InventoriesModel.Equipment;
using SavingSystem;
using Utils;

namespace RPG.GameplayCore.Combat
{
    [RequireComponent(typeof(ActionScheduler), typeof(Animator), typeof(Mover))]
    [RequireComponent(typeof(BaseStats))]
    public class Fighter : MonoBehaviour, IAction, ISavable
    {
        [Required]
        [SerializeField]
        private Transform rightHand, leftHand;

        [SerializeField]
        private WeaponConfig defaultWeaponConfig;

        private WeaponConfig _currentWeaponConfig;
        private LazyValue<Weapon> _currentWeapon;

        private Health _target;
        private Equipment _equipment;

        private float _timeSinceLastAttack = Mathf.Infinity;

        private BaseStats _baseStats;

        private Mover _movementSystem;
        private ActionScheduler _actionScheduler;

        private Animator _animator;
        private static readonly int AttackId = Animator.StringToHash("Attack");
        private static readonly int StopAttackingId = Animator.StringToHash("StopAttacking");

        public Health CombatTarget => _target;

        private void Awake()
        {
            _movementSystem = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
            _baseStats = GetComponent<BaseStats>();

            _currentWeaponConfig = defaultWeaponConfig;
            _currentWeapon = new LazyValue<Weapon>(EquipDefaultWeapon);

            _equipment = GetComponent<Equipment>();
            if (_equipment != null)
            {
                _equipment.EquipmentUpdated += UpdateWeapon;
            }
        }

        private void UpdateWeapon()
        {
            var currentEquipWeapon = _equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if (currentEquipWeapon != null)
            {
                EquipWeapon(currentEquipWeapon);
            }
            else
            {
                EquipDefaultWeapon();
            }
        }

        private void Start()
        {
            // restore default weapon
            _currentWeapon.ForceInit();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_target == null) return;

            if (!InAttackRange(_target.transform))
                _movementSystem.MoveTo(_target.transform.position);
            else
            {
                _movementSystem.Cancel();
                AttackBehaviour();
            }
        }

        private Weapon EquipDefaultWeapon()
        {
            _currentWeaponConfig = defaultWeaponConfig;
            return AttachWeapon(defaultWeaponConfig);
        }

        private void EquipWeapon(WeaponConfig weaponConfig)
        {
            if (weaponConfig == null) return;

            _currentWeaponConfig = weaponConfig;
            _currentWeapon.Value = AttachWeapon(weaponConfig);
        }

        private Weapon AttachWeapon(WeaponConfig weaponConfig)
        {
            if (weaponConfig == null) return null;

            return weaponConfig.CreateWeapon(leftHand, rightHand, _animator);
        }

        private void AttackBehaviour()
        {
            if (!CanAttack(_target.gameObject)) return;

            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack >= _currentWeaponConfig.TimeBetweenAttacks)
            {
                ResetTriggerAttack();
                _animator.SetTrigger(AttackId);

                _timeSinceLastAttack = 0f;
            }
        }

        /// <summary>
        /// Reset the previous state of attack trigger to avoid lagging before new attack.
        /// </summary>
        private void ResetTriggerAttack()
        {
            _animator.ResetTrigger(StopAttackingId);
        }

        public void Attack(GameObject combatTarget)
        {
            _target = combatTarget.GetComponent<Health>();

            _actionScheduler.StartAction(this);
        }

        public bool CanAttack(GameObject target)
        {
            Health healthComponent;
            if (target == null || (healthComponent = target.GetComponent<Health>()) == null)
                return false;

            if (!_movementSystem.CanMoveTo(target.transform.position) && !InAttackRange(target.transform)) return false;
            return !healthComponent.IsDead;
        }

        private bool InAttackRange(Transform target)
        {
            if (target == null) return false;

            return Vector3.Distance(transform.position, target.position) < _currentWeaponConfig.WeaponRange;
        }

        public void Cancel()
        {
            ResetTriggerAttack();
            _animator.SetTrigger(StopAttackingId);
            _target = null;
        }

        /// <summary>
        /// Animation event to apply damage
        /// </summary>
        private void OnHit()
        {
            if (_target == null || _target.IsDead) return;

            float damage = _baseStats.GetStat(Stats.Stats.Damage);

            if (_currentWeapon.Value != null)
                _currentWeapon.Value.OnHit();

            if (_currentWeaponConfig.HasProjectile)
                _currentWeaponConfig.LaunchProjectile(leftHand, rightHand, _target, gameObject, damage);
            else
                // Apply damage for non projectile weapons
                _target.TakeDamage(damage, gameObject);
        }

        public object CaptureState()
        {
            return _currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            if (!(state is string weaponName)) return;
            WeaponConfig weaponConfig = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weaponConfig);
        }
    }
}