using System;
using System.Collections.Generic;
using RPG.GameplayCore.Stats.Exceptions;
using UnityEngine;

namespace RPG.GameplayCore.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "RPG Project/Stats/New Progression", order = 0)]
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

            [Header("Properties By Level")]
            [SerializeField]
            internal float[] valuesOnLevels;
        }

        [SerializeField]
        private CharacterProgressionClass[] characterProgressions;

        private Dictionary<CharacterClass, Dictionary<Stats, float[]>> _lookupStats;

        private Dictionary<CharacterClass, Dictionary<Stats, float[]>> BuildLookup()
        {
            var lookup = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();

            foreach (var progression in characterProgressions)
            {
                // create inner dictionary for type of stats and values on levels
                var stats = new Dictionary<Stats, float[]>();

                foreach (ProgressionStats progressionStat in progression.stats)
                {
                    stats[progressionStat.statsType] = progressionStat.valuesOnLevels;
                }

                // link inner dictionary by characterClass key
                lookup[progression.characterClass] = stats;
            }

            return lookup;
        }

        public float GetStat(Stats statsType, CharacterClass characterClass, int level)
        {
            if (_lookupStats == null)
                _lookupStats = BuildLookup();

            try
            {
                return _lookupStats[characterClass][statsType][level - 1];
            }
            catch (Exception ex)
            {
                throw new ProgressionStatException(
                    $"Can't get stat for: statsType={statsType}, characterClass={characterClass}, level={level}", ex);
            }
        }

        public int GetNumberOfLevels(Stats statsType, CharacterClass characterClass)
        {
            if (_lookupStats == null)
                _lookupStats = BuildLookup();

            try
            {
                return _lookupStats[characterClass][statsType].Length;
            }
            catch (Exception ex)
            {
                throw new ProgressionStatException(
                    $"Can't get stat for: statsType={statsType}, characterClass={characterClass}", ex);
            }
        }
    }
}