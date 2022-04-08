using System;
using System.Linq;
using RPG.GameplayCore.Attributes;
using UnityEngine;
using Utils;

namespace RPG.GameplayCore.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField]
        private int progressLevel = 1;

        [SerializeField]
        private CharacterClass characterClass;

        [SerializeField]
        private Progression progression;

        [SerializeField]
        private GameObject levelUpEffect;

        [SerializeField]
        [Tooltip(
            "Using additional modifiers for components (ex. if false, stats will be only loaded from Progression Scriptable object, without any additional modifiers)")]
        private bool shouldUseModifiers;

        /// <summary>
        /// Null if it is not a Player 
        /// </summary>
        private Experience _experience;

        private LazyValue<int> _currentLevel;

        public event Action OnLevelUp;

        private void Awake()
        {
            _experience = GetComponent<Experience>();

            _currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            _currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (_experience != null)
                _experience.OnExperienceGained += UpdateLevel;
        }

        private void OnDisable()
        {
            if (_experience != null)
                _experience.OnExperienceGained -= UpdateLevel;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > _currentLevel.Value)
            {
                _currentLevel.Value = newLevel;
                ShowLevelUpAffect();

                OnLevelUp?.Invoke();
            }
        }

        private void ShowLevelUpAffect()
        {
            Instantiate(levelUpEffect, transform);
        }

        public float GetStat(Stats statsType)
        {
            return (GetBaseStat(statsType) + GetAdditiveModifier(statsType)) *
                   (1 + GetPercentageModifier(statsType) / 100);
        }

        private float GetPercentageModifier(Stats statsType)
        {
            if (!shouldUseModifiers) return 0f;

            float percentage = 0f;
            foreach (var modifyProvider in GetComponents<IModifyProvider>())
            {
                percentage += modifyProvider.GetPercentageModifier(statsType).Sum();
            }

            return percentage;
        }

        private float GetBaseStat(Stats statType)
        {
            return progression.GetStat(statType, characterClass, _currentLevel.Value);
        }

        private float GetAdditiveModifier(Stats statsType)
        {
            if (!shouldUseModifiers) return 0f;

            float additiveSum = 0;
            foreach (var modifyProvider in GetComponents<IModifyProvider>())
            {
                additiveSum += modifyProvider.GetAdditiveModifier(statsType).Sum();
            }

            return additiveSum;
        }

        public int GetLevel()
        {
            return _currentLevel.Value;
        }

        private int CalculateLevel()
        {
            if (!gameObject.CompareTag("Player")) return progressLevel;

            float currentXp = _experience.ExperiencePoints;
            int penultimateLevel = progression.GetNumberOfLevels(Stats.ExperienceToLevelUp, characterClass);

            for (int level = 1; level < penultimateLevel; level++)
            {
                float pointsToLevelUp = progression.GetStat(Stats.ExperienceToLevelUp, characterClass, level);
                if (currentXp < pointsToLevelUp)
                    return level;
            }

            return penultimateLevel + 1;
        }
    }
}