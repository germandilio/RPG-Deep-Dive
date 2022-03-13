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

        public float GetStat(Stats statsType)
        {
            return progression.GetStat(statsType, characterClass, progressLevel);
        }
    }
}