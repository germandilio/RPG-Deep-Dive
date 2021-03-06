using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RPG.GameplayCore.Attributes;
using Unity.Services.Analytics;
using UnityEngine;
using Utils;
using Utils.UI.Hint;

namespace RPG.GameplayCore.Stats
{
    public class BaseStats : MonoBehaviour
    {
        private const string LevelUpHint = "Уровень повышен";

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

        public event Action LevelUp;

        public float CurrentExperiencePoints => _experience?.ExperiencePoints ?? 0f;

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

        private void UpdateLevel(bool isSilent)
        {
            int newLevel = CalculateLevel();
            if (newLevel > _currentLevel.Value)
            {
                _currentLevel.Value = newLevel;

                if (!isSilent)
                {
                    ShowLevelUpAffect();
                }

                LevelUp?.Invoke();

                // analytics
                var parameters = new Dictionary<string, object>()
                {
                    {"userLevel", _currentLevel.Value}
                };

                AnalyticsService.Instance.CustomData("level_up", parameters);
            }
        }

        private void ShowLevelUpAffect()
        {
            Instantiate(levelUpEffect, transform);
            HintSpawner.Spawn(LevelUpHint);
        }

        public float GetStat(Stats statsType)
        {
            return (GetBaseStat(statsType) + GetAdditiveModifier(statsType)) *
                   (1 + GetPercentageModifier(statsType) / 100);
        }

        public float GetStatOnPreviousLevel(Stats statType)
        {
            int previousLevel = _currentLevel.Value > 1 ? _currentLevel.Value - 1 : 1;
            float baseStat = progression.GetStat(statType, characterClass, previousLevel);

            return (baseStat + GetAdditiveModifier(statType)) *
                   (1 + GetPercentageModifier(statType) / 100);
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