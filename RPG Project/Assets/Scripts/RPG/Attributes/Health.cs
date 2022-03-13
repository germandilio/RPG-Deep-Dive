using System;
using RPG.Core;
using RPG.Stats;
using Saving;
using UnityEngine;

namespace RPG.Attributes
{
    [RequireComponent(typeof(ActionScheduler), typeof(Rigidbody), typeof(CapsuleCollider))]
    [RequireComponent(typeof(BaseStats))]
    public class Health : MonoBehaviour, ISaveable
    {
        private BaseStats _baseStats;
        
        private float _currentHealthPoints;

        private Animator _animator;
        private static readonly int DeadId = Animator.StringToHash("Dead");

        private GameObject _instigator = null;
        
        public bool IsDead { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _baseStats = GetComponent<BaseStats>();
            _currentHealthPoints = _baseStats.GetStat(Stats.Stats.Health);
        }

        public void TakeDamage(float damage, GameObject instigator)
        {
            _instigator = instigator;
            
            _currentHealthPoints = Math.Max(_currentHealthPoints - damage, 0);
            // Debug.Log(_currentHealthPoints);
            if (_currentHealthPoints == 0)
            {
                Die();
                AwardXpToInstigator();
            }
        }

        private void AwardXpToInstigator()
        {
            if (_instigator == null) return;
         
            // award XP to instigator
            float pointToAdd = _baseStats.GetStat(Stats.Stats.ExperienceRewards);
            _instigator.GetComponent<Experience>()?.AwardXp(pointToAdd);

        }

        public int GetHealthPercentage()
        {
            return Mathf.RoundToInt(_currentHealthPoints / _baseStats.GetStat(Stats.Stats.Health) * 100);
        }

        private void Die()
        {
            if (IsDead) return;

            _animator.SetTrigger(DeadId);
            IsDead = true;

            // TODO set enable to false on enemy capsule collider
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return _currentHealthPoints;
        }

        public void RestoreState(object state)
        {
            if (state is float savedHealthPoints)
            {
                // TODO debug
                print("restore health");
                _currentHealthPoints = savedHealthPoints;
            }

            if (_currentHealthPoints == 0)
                Die();
        }
    }
}