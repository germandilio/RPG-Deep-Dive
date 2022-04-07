using System;
using SavingSystem;
using UnityEngine;

namespace RPG.GameplayCore.Attributes
{
    public class Experience : MonoBehaviour, ISavable
    {
        public event Action OnExperienceGained;

        private float _experiencePoints;

        public float ExperiencePoints => _experiencePoints;

        public void AwardXp(float pointsToAdd)
        {
            _experiencePoints += pointsToAdd;
            OnExperienceGained?.Invoke();
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
                OnExperienceGained?.Invoke();
            }
        }
    }
}