using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator),
        typeof(ActionScheduler))]
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent _navMeshAgent;

        private Animator _animator;

        private ActionScheduler _actionScheduler;

        private static readonly int ForwardSpeedId = Animator.StringToHash("ForwardSpeed");

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination);
        }
        
        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.SetDestination(destination);
            _navMeshAgent.isStopped = false;
        }
        
        private void UpdateAnimator()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(_navMeshAgent.velocity);
            _animator.SetFloat(ForwardSpeedId, localVelocity.z);
        }
        
        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }
    }
}