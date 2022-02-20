using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using Saving;

namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(ActionScheduler))]
    [RequireComponent(typeof(Health))]
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField]
        private float maxSpeed = 6f;

        private NavMeshAgent _navMeshAgent;
        private ActionScheduler _actionScheduler;
        private Health _healthSystem;

        private Animator _animator;
        private static readonly int ForwardSpeedId = Animator.StringToHash("ForwardSpeed");

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _healthSystem = GetComponent<Health>();

            _navMeshAgent.speed = maxSpeed;
        }

        private void Update()
        {
            // disable navMeshAgent after death
            _navMeshAgent.enabled = !_healthSystem.IsDead;

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFactor = 1f)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination, speedFactor);
        }

        public void MoveTo(Vector3 destination, float speedFactor = 1f)
        {
            _navMeshAgent.SetDestination(destination);
            _navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFactor);
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

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            if (state is SerializableVector3 savedPosition)
            {
                // TODO debug
                print("restore position");
                _navMeshAgent.Warp(savedPosition.ToVector());
            }
        }
    }
}