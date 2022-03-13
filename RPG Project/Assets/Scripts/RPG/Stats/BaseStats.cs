using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField]
        private int progressLevel = 1;

        [SerializeField]
        private CharacterClass characterClass;

        [SerializeField]
        private Progression progression = null;

        [SerializeField]
        private GameObject levelUpEffect;

        // null if it is not a Player
        private Experience _experience;
        private int _currentLevel;

        public event Action OnLevelUp;
        
        private void Awake()
        {
            _experience = GetComponent<Experience>();
            _currentLevel = CalculateLevel();
        }

        private void OnEnable()
        {
            if (_experience != null)
                _experience.ExperienceGained += UpdateLevel;
        }

        private void OnDisable()
        {
            if (_experience != null)
                _experience.ExperienceGained -= UpdateLevel;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > _currentLevel)
            {
                _currentLevel = newLevel;
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
            return progression.GetStat(statsType, characterClass, _currentLevel);
        }

        public int GetLevel()
        {
            return _currentLevel;
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