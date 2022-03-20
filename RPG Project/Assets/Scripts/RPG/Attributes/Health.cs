using System;
using RPG.Core;
using RPG.Stats;
using Saving;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace RPG.Attributes
{
    [RequireComponent(typeof(ActionScheduler), typeof(Rigidbody), typeof(CapsuleCollider))]
    [RequireComponent(typeof(BaseStats))]
    public class Health : MonoBehaviour, ISavable
    {
        [SerializeField]
        private float levelUpHealthPercentage = 70;

        [Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        [SerializeField]
        private TakeDamageEvent takeDamage;

        private BaseStats _baseStats;

        private LazyValue<float> _currentHealthPoints;

        private Animator _animator;
        private static readonly int DeadId = Animator.StringToHash("Dead");

        private GameObject _instigator;

        public bool IsDead { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _baseStats = GetComponent<BaseStats>();

            _currentHealthPoints = new LazyValue<float>(InitializeHealthPoints);
        }

        private void Start()
        {
            _currentHealthPoints.ForceInit();
        }

        private float InitializeHealthPoints() => _baseStats.GetStat(Stats.Stats.Health);

        private void OnEnable()
        {
            _baseStats.OnLevelUp += NormalizeHealthPercentage;
        }

        private void OnDisable()
        {
            _baseStats.OnLevelUp -= NormalizeHealthPercentage;
        }

        public float GetCurrentHealth()
        {
            return _currentHealthPoints.Value;
        }

        public float GetMaxHealth()
        {
            return _baseStats.GetStat(Stats.Stats.Health);
        }

        private void NormalizeHealthPercentage()
        {
            _currentHealthPoints.Value = _baseStats.GetStat(Stats.Stats.Health) * (levelUpHealthPercentage / 100);
        }

        public void TakeDamage(float damage, GameObject instigator)
        {
            _instigator = instigator;

            _currentHealthPoints.Value = Math.Max(_currentHealthPoints.Value - damage, 0);
            takeDamage?.Invoke(damage);

            if (_currentHealthPoints.Value == 0)
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
            return _currentHealthPoints.Value;
        }

        public void RestoreState(object state)
        {
            if (state is float savedHealthPoints)
            {
                // TODO debug
                print("restore health");
                _currentHealthPoints.Value = savedHealthPoints;
            }

            if (_currentHealthPoints.Value == 0)
                Die();
        }
    }
}