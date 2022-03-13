using System;
using Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        public event Action ExperienceGained;
        
        private float _experiencePoints = 0;

        public float ExperiencePoints => _experiencePoints;
        
        public void AwardXp(float pointsToAdd)
        {
            _experiencePoints += pointsToAdd;
            ExperienceGained?.Invoke();
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
                ExperienceGained?.Invoke();
            }
        }
    }
}