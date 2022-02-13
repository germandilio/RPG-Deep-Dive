using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Mover), typeof(ActionScheduler),
        typeof(Animator))]
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField]
        private float weaponRange = 2f;
        [SerializeField]
        private float timeBetweenAttacks = 2f;
        private float _timeSinceLastAttack = 0f;    
        
        private Mover _movementSystem;
        private ActionScheduler _actionScheduler;
        
        private Transform _target;

        private Animator _animator;
        private static readonly int AttackId = Animator.StringToHash("Attack");
        
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
            
            if (Vector3.Distance(transform.position, _target.position) >= weaponRange)
                _movementSystem.MoveTo(_target.position);
            else
            {
                _movementSystem.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (_timeSinceLastAttack >= timeBetweenAttacks)
            {
                _animator.SetTrigger(AttackId);
                _timeSinceLastAttack = 0f;
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            Debug.Log("you're attacking");
            _actionScheduler.StartAction(this);
            
            _target = combatTarget.transform;
        }

        public void Cancel()
        {
            _target = null;
        }

        /// <summary>
        /// Animation event
        /// </summary>
        private void Hit()
        {
            
        }
    }
}
