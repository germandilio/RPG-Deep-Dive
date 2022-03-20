using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Attributes;
using RPG.Movement;
using RPG.Stats;
using Saving;
using Utils;

namespace RPG.Combat
{
    [RequireComponent(typeof(ActionScheduler), typeof(Animator), typeof(Mover))]
    [RequireComponent(typeof(BaseStats))]
    public class Fighter : MonoBehaviour, IAction, ISavable, IModifyProvider
    {
        [SerializeField]
        private Transform rightHand, leftHand;

        [SerializeField]
        private Weapon defaultWeapon;

        private LazyValue<Weapon> _currentWeapon;

        private Health _target;

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

            _currentWeapon = new LazyValue<Weapon>(EquipDefaultWeapon);
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

            if (Vector3.Distance(transform.position, _target.transform.position) >= _currentWeapon.Value.WeaponRange)
                _movementSystem.MoveTo(_target.transform.position);
            else
            {
                _movementSystem.Cancel();
                AttackBehaviour();
            }
        }

        private Weapon EquipDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        public void EquipWeapon(Weapon weapon)
        {
            AttachWeapon(weapon);
            _currentWeapon.Value = weapon;
        }

        private void AttachWeapon(Weapon weapon)
        {
            if (weapon == null) return;

            weapon.CreateWeapon(leftHand, rightHand, _animator);
        }

        private void AttackBehaviour()
        {
            if (!CanAttack(_target.gameObject)) return;

            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack >= _currentWeapon.Value.TimeBetweenAttacks)
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
            //TODO убрать
            Debug.Log("you're attacking");
            _target = combatTarget.GetComponent<Health>();

            _actionScheduler.StartAction(this);
        }

        public bool CanAttack(GameObject target)
        {
            Health healthComponent;
            if (target == null || (healthComponent = target.GetComponent<Health>()) == null)
            {
                return false;
            }

            return !healthComponent.IsDead;
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
            // TODO damage weapon
            if (_currentWeapon.Value.HasProjectile)
                _currentWeapon.Value.LaunchProjectile(leftHand, rightHand, _target, gameObject, damage);
            else
                // Apply damage for non projectile weapons
                _target.TakeDamage(damage, gameObject);
        }

        public object CaptureState()
        {
            return _currentWeapon.Value.name;
        }

        public void RestoreState(object state)
        {
            if (!(state is String weaponName)) return;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }

        public IEnumerable<float> GetAdditiveModifier(Stats.Stats stats)
        {
            if (stats == Stats.Stats.Damage)
            {
                yield return _currentWeapon.Value.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stats.Stats stats)
        {
            if (stats == Stats.Stats.Damage)
            {
                yield return _currentWeapon.Value.PercentageBonus;
            }
        }
    }
}