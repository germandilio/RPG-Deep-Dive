using RPG.Combat;
using RPG.Core;
using RPG.Attributes;
using RPG.Movement;
using UnityEngine;
using Utils;

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

        [SerializeField]
        [Tooltip("Max time to wait after player go out chase distance, before move to guard place")]
        private float suspicionTime = 4f;

        [SerializeField]
        [Tooltip("Time when enemy stops on every waypoint")]
        private float dwellTime = 5f;

        [SerializeField]
        private float aggravatedTime = 5f;

        [SerializeField]
        private float shoutDistance = 5f;
        
        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private float _timeOnCurrentWaypoint = Mathf.Infinity;
        private float _timeSinceAggravated = Mathf.Infinity;

        private LazyValue<Vector3> _guardPosition;

        private Fighter _fighterSystem;
        private Health _healthSystem;
        private Mover _moverSystem;
        private ActionScheduler _actionScheduler;

        private GameObject _player;

        private void Awake()
        {
            _fighterSystem = GetComponent<Fighter>();
            _healthSystem = GetComponent<Health>();
            _moverSystem = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();

            _player = GameObject.FindWithTag("Player");

            _guardPosition = new LazyValue<Vector3>(InitializeGuardPosition);
        }

        private void Start()
        {
            _guardPosition.ForceInit();
        }

        private Vector3 InitializeGuardPosition() => transform.position;

        private void Update()
        {
            if (_healthSystem.IsDead) return;

            if (IsAggravated() && _fighterSystem.CanAttack(_player))
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
            _timeSinceAggravated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            _fighterSystem.Cancel();

            Vector3 nextPosition = _guardPosition.Value;
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

            AggravateNearbyEnemies();
        }

        private void AggravateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0.5f);
            foreach (RaycastHit hit in hits)
            {
                var aiController = hit.transform.GetComponent<AIController>();
                if (aiController != null)
                    aiController.Aggravate();
            }
        }

        private bool IsAggravated()
        {
            if (_timeSinceAggravated < aggravatedTime) return true;
            return Vector3.Distance(transform.position, _player.transform.position) <= chaseDistance;
        }

        public void Aggravate() => _timeSinceAggravated = 0f;

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