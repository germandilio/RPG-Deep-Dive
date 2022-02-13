using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private float initialHealthPoints = 100f;

        public bool IsDead { get; private set; }
        private float _currentHealthPoints;

        private Animator _animator;
        private static readonly int DeadId = Animator.StringToHash("Dead");

        private void Awake()
        {
            _currentHealthPoints = initialHealthPoints;
            _animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            _currentHealthPoints = Math.Max(_currentHealthPoints - damage, 0);
            // Debug.Log(_currentHealthPoints);
            if (_currentHealthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (IsDead) return;
            
            _animator.SetTrigger(DeadId);
            IsDead = true;
            
            // TODO (13/02/2022) set enable to false on enemy capsule collider
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}