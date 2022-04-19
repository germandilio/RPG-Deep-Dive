using RPG.GameplayCore.Attributes;
using RPG.GameplayCore.Core;
using SavingSystem;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace RPG.GameplayCore.Movement
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(ActionScheduler))]
    public class Mover : MonoBehaviour, IAction, ISavable
    {
        [SerializeField]
        private float maxSpeed = 6f;

        [SerializeField]
        private float maxNavPathLength = 35f;

        private NavMeshAgent _navMeshAgent;
        private ActionScheduler _actionScheduler;

        private Animator _animator;
        private static readonly int ForwardSpeedId = Animator.StringToHash("ForwardSpeed");

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();

            _navMeshAgent.speed = maxSpeed;
        }

        private void Update()
        {
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

        public bool CanMoveTo(Vector3 targetPosition)
        {
            var path = new NavMeshPath();
            bool hasPath =
                NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
            if (!hasPath || path.status != NavMeshPathStatus.PathComplete) return false;
            if (NavMeshExtensions.CalculateLength(path) > maxNavPathLength) return false;

            return true;
        }

        private void UpdateAnimator()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(_navMeshAgent.velocity);
            _animator.SetFloat(ForwardSpeedId, localVelocity.z);
        }

        public void Cancel()
        {
            if (_navMeshAgent.isActiveAndEnabled)
                _navMeshAgent.isStopped = true;
        }

        /// <summary>
        /// Disable navMeshAgent after death.
        /// </summary>
        /// <remarks>
        /// Event function.
        /// </remarks>
        public void DisableOnDeath()
        {
            _navMeshAgent.enabled = false;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            if (state is SerializableVector3 savedPosition)
            {
                _navMeshAgent.Warp(savedPosition.ToVector());
            }
        }
    }
}