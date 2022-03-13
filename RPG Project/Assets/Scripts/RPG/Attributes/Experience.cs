using Saving;
using UnityEngine;

namespace RPG.Attributes
{
    public class Experience : MonoBehaviour, ISaveable
    {
        private float _experiencePoints = 0;

        public float ExperiencePoints => _experiencePoints;
        
        public void AwardXp(float pointsToAdd)
        {
            _experiencePoints += pointsToAdd;
        }

        public object CaptureState()
        {
            return _experiencePoints;
        }

        public void RestoreState(object state)
        {
            if (state is float savedPoints) 
                _experiencePoints = savedPoints;
        }
    }
}