using RPG.GameplayCore.Stats;
using TMPro;
using UnityEngine;

namespace RPG.GameplayCore.Attributes
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LevelDisplay : MonoBehaviour
    {
        private BaseStats _playerBaseStats;
        private TextMeshProUGUI _displayText;

        private void Awake()
        {
            _playerBaseStats = GameObject.FindWithTag("Player")?.GetComponent<BaseStats>();
            _displayText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (_playerBaseStats == null)
                _displayText.text = "N/A";
            else
                _displayText.text = _playerBaseStats.GetLevel().ToString();
        }
    }
}