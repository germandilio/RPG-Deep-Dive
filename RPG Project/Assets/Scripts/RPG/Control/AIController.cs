using System;
using RPG.Combat;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Fighter), typeof(Health))]
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private float chaseDistance = 5f;

        private Fighter _fighterSystem;
        private Health _healthSystem;
        
        private GameObject _player;

        private void Awake()
        {
            _fighterSystem = GetComponent<Fighter>();
            _healthSystem = GetComponent<Health>();
            
            _player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            if (_healthSystem.IsDead) return;
            
            if (InAttackRange() && _fighterSystem.CanAttack(_player))
            {
                _fighterSystem.Attack(_player);
            }
            else
            {
                _fighterSystem.Cancel();
            }
        }

        private bool InAttackRange()
        {
            return Vector3.Distance(transform.position, _player.transform.position) <= chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}