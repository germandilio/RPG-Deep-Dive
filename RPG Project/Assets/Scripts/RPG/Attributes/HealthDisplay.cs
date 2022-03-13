using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    [RequireComponent(typeof(Text))]
    public class HealthDisplay : MonoBehaviour
    {
        private Health _health;

        private Text _displayText;
        
        private void Awake()
        {
            _health = GameObject.FindWithTag("Player")?.GetComponent<Health>();
            _displayText = GetComponent<Text>();
        }

        private void Update()
        {
            _displayText.text = $"{_health.GetHealthPercentage()}%";
        }
    }
}