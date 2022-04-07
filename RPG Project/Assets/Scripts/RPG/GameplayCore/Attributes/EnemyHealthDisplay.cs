using RPG.GameplayCore.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.GameplayCore.Attributes
{
    [RequireComponent(typeof(Text))]
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter _fighter;
        private Text _displayText;

        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player")?.GetComponent<Fighter>();
            _displayText = GetComponent<Text>();
        }

        private void Update()
        {
            if (_fighter.CombatTarget == null)
                _displayText.text = "N/A";
            else
                _displayText.text =
                    $"{_fighter.CombatTarget.GetCurrentHealth():0}/{_fighter.CombatTarget.GetMaxHealth():0}";
        }
    }
}