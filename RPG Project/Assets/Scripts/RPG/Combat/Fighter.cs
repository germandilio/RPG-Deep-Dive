using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Mover), typeof(ActionScheduler))]
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField]
        private float weaponRange = 2f;
        
        [SerializeField]
        private Mover movementSystem;
        [SerializeField]
        private ActionScheduler actionScheduler;
        
        private Transform _target;

        private void Awake()
        {
            movementSystem = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            if (_target == null) return;
            
            if (Vector3.Distance(transform.position, _target.position) >= weaponRange)
                movementSystem.MoveTo(_target.position);
            else
                movementSystem.Cancel();
        }

        public void Attack(CombatTarget combatTarget)
        {
            Debug.Log("you're attacking");
            actionScheduler.StartAction(this);
            _target = combatTarget.transform;
        }

        public void Cancel()
        {
            _target = null;
        }
    }
}
