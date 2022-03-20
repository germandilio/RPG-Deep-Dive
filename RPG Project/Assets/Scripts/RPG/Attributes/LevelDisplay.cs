using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    [RequireComponent(typeof(Text))]
    public class LevelDisplay : MonoBehaviour
    {
        private BaseStats _playerBaseStats;
        private Text _displayText;

        private void Awake()
        {
            _playerBaseStats = GameObject.FindWithTag("Player")?.GetComponent<BaseStats>();
            _displayText = GetComponent<Text>();
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