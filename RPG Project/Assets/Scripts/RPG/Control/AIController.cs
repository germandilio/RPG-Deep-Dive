using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Fighter), typeof(Health), typeof(Mover))]
    [RequireComponent(typeof(ActionScheduler))]
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private PatrolPath patrolPath;

        [SerializeField]
        [Range(0, 1)]
        [Tooltip("Character speed will be multiplied by this factor, when enemy patrolling.")]
        private float patrolSpeedFactor = 0.2f;

        [SerializeField]
        private float chaseDistance = 5f;

        [SerializeField]
        private float waypointTolerance = 0.5f;

        private Vector3 _guardPosition;

        [SerializeField]
        [Tooltip("Max time to wait after player go out chase distance, before move to guard place")]
        private float suspicionTime = 4f;

        [SerializeField]
        [Tooltip("Time when enemy stops on every waypoint")]
        private float dwellTime = 5f;

        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private float _timeOnCurrentWaypoint = Mathf.Infinity;

        private Fighter _fighterSystem;
        private Health _healthSystem;
        private Mover _moverSystem;
        private ActionScheduler _actionScheduler;

        private GameObject _player;

        private void Awake()
        {
            _guardPosition = transform.position;

            _fighterSystem = GetComponent<Fighter>();
            _healthSystem = GetComponent<Health>();
            _moverSystem = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();

            _player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            if (_healthSystem.IsDead) return;

            if (InAttackRange() && _fighterSystem.CanAttack(_player))
                AttackBehaviour();
            else if (_timeSinceLastSawPlayer < suspicionTime)
                SuspicionBehaviour();
            else
                PatrolBehaviour();

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeOnCurrentWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            _fighterSystem.Cancel();

            Vector3 nextPosition = _guardPosition;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    patrolPath.CycleWaypoint();
                    _timeOnCurrentWaypoint = 0f;
                }

                nextPosition = patrolPath.GetCurrentWaypoint();
            }

            if (_timeOnCurrentWaypoint >= dwellTime)
            {
                // move enemy to next waypoint
                _moverSystem.StartMoveAction(nextPosition, patrolSpeedFactor);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, patrolPath.GetCurrentWaypoint());
            return distanceToWaypoint <= waypointTolerance;
        }

        private void SuspicionBehaviour()
        {
            _actionScheduler.CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _timeSinceLastSawPlayer = 0f;
            _fighterSystem.Attack(_player);
        }

        private bool InAttackRange()
        {
            return Vector3.Distance(transform.position, _player.transform.position) <= chaseDistance;
        }

        /// <summary>
        /// Draw enemy chasing sphere 
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}