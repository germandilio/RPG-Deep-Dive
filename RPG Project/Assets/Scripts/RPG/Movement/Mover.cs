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
        [SerializeField]
        private NavMeshAgent navMeshAgent;

        [SerializeField]
        private Animator animator ;

        private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }
        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.SetDestination(destination);
            navMeshAgent.isStopped = false;
        }
        
        private void UpdateAnimator()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
            animator.SetFloat(ForwardSpeed, localVelocity.z);
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
    }
}