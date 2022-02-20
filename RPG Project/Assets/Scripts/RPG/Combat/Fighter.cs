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
        [Tooltip("Distance to enemy, where player should stop when attacking")]
        private float weaponRange = 2f;

        [SerializeField]
        [Tooltip("Damage, which player apply to Combat target")]
        private float weaponDamage = 25f;

        [SerializeField]
        [Tooltip("Time in seconds between player attacks")]
        private float timeBetweenAttacks = 2f;

        private float _timeSinceLastAttack = Mathf.Infinity;

        private Mover _movementSystem;
        private ActionScheduler _actionScheduler;

        private Health _target;

        private Animator _animator;
        private static readonly int AttackId = Animator.StringToHash("Attack");
        private static readonly int StopAttackingId = Animator.StringToHash("StopAttacking");

        private void Awake()
        {
            _movementSystem = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_target == null) return;

            if (Vector3.Distance(transform.position, _target.transform.position) >= weaponRange)
                _movementSystem.MoveTo(_target.transform.position);
            else
            {
                _movementSystem.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (!CanAttack(_target.gameObject)) return;

            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack >= timeBetweenAttacks)
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
            // Apply damage
            if (_target == null) return;
            ;
            if (!_target.IsDead)
                _target.TakeDamage(weaponDamage);
        }
    }
}