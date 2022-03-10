using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(ActionScheduler), typeof(Animator), typeof(Mover))]
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField]
        private Transform rightHand, leftHand;

        [SerializeField]
        private Weapon defaultWeapon;

        private Weapon _currentWeapon = null;

        private Health _target;

        private float _timeSinceLastAttack = Mathf.Infinity;

        private Mover _movementSystem;
        private ActionScheduler _actionScheduler;

        private Animator _animator;
        private static readonly int AttackId = Animator.StringToHash("Attack");
        private static readonly int StopAttackingId = Animator.StringToHash("StopAttacking");

        private void Awake()
        {
            _movementSystem = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_target == null) return;

            if (Vector3.Distance(transform.position, _target.transform.position) >= _currentWeapon.WeaponRange)
                _movementSystem.MoveTo(_target.transform.position);
            else
            {
                _movementSystem.Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) return;

            _currentWeapon = weapon;
            weapon.CreateWeapon(leftHand, rightHand, _animator);
        }

        private void AttackBehaviour()
        {
            if (!CanAttack(_target.gameObject)) return;

            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack >= _currentWeapon.TimeBetweenAttacks)
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

            if (_currentWeapon.HasProjectile)
                _currentWeapon.LaunchProjectile(leftHand, rightHand, _target);
            else
                // Apply damage for non projectile weapons
                _target.TakeDamage(_currentWeapon.WeaponDamage);
        }
    }
}