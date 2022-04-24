using System;
using SavingSystem;
using UnityEngine;

namespace RPG.GameplayCore.Attributes
{
    public class Experience : MonoBehaviour, ISavable
    {
        /// <summary>
        /// Boolean flag, if true - silent level update, if false - general level update
        /// </summary>
        public event Action<bool> OnExperienceGained;

        private float _experiencePoints;

        public float ExperiencePoints => _experiencePoints;

        public void AwardXp(float pointsToAdd)
        {
            _experiencePoints += pointsToAdd;
            OnExperienceGained?.Invoke(false);
        }

        public object CaptureState()
        {
            return _experiencePoints;
        }

        public void RestoreState(object state)
        {
            if (state is float savedPoints)
            {
                _experiencePoints = savedPoints;
                OnExperienceGained?.Invoke(true);
            }
        }
    }
}