using System;
using System.Linq;
using RPG.Stats.Exceptions;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [Serializable]
        class CharacterProgressionClass
        {
            [SerializeField]
            internal CharacterClass characterClass;

            [SerializeField]
            internal ProgressionStats[] stats;
        }

        [Serializable]
        class ProgressionStats
        {
            [SerializeField]
            internal Stats statsType;

            [SerializeField]
            internal float[] valuesOnLevels;
        }
        
        [SerializeField]
        private CharacterProgressionClass[] characterProgressions = null;

        public float GetStat(Stats statsType, CharacterClass characterClass, int level)
        {
            try
            {
                var statsByCharacter = characterProgressions
                    .First(progression => progression.characterClass == characterClass).stats;
                var statHealth = statsByCharacter.First(progressionStats => progressionStats.statsType == statsType);
                return statHealth.valuesOnLevels[level - 1];
            }
            catch (Exception ex)
            {
                throw new ProgressionStatException($"Can't get stat for: statsType={statsType}, characterClass={characterClass}, level={level}", ex);
            }
        }
    }
}