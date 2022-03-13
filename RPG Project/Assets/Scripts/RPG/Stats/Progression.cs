using System;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField]
        private CharacterProgressionClass[] characterProgressions = null;

        [Serializable]
        class CharacterProgressionClass
        {
            [SerializeField]
            internal CharacterClass characterClass;

            [SerializeField]
            internal float[] health;
        }

        public float GetHealth(CharacterClass characterClass, int level)
        {
            var result = characterProgressions.FirstOrDefault(progression => progression.characterClass == characterClass);
            if (result == null)
                return 0;

            return result.health[level - 1];
        }
    }
}