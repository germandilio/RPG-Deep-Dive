using System;
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
            foreach (var progression in characterProgressions)
            {
                if (progression.characterClass == characterClass)
                {
                    return progression.health[level - 1];
                }
            }
            return 0;
        }
    }
}