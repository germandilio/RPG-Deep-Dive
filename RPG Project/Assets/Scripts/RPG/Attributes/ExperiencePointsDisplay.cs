using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    [RequireComponent(typeof(Text))]
    public class ExperiencePointsDisplay : MonoBehaviour
    {
        private Text _displayText;
        private Experience _playerExperience;

        private void Awake()
        {
            _playerExperience = GameObject.FindWithTag("Player")?.GetComponent<Experience>();
            _displayText = GetComponent<Text>();
        }

        private void Update()
        {
            if (_playerExperience == null)
                _displayText.text = "N/A";
            else
                _displayText.text = $"{_playerExperience.ExperiencePoints} points";
        }
    }
}