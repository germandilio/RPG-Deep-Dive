using TMPro;
using UnityEngine;

namespace RPG.GameplayCore.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textComponent;

        public void SetText(float damage)
        {
            textComponent.text = $"{damage:0}";
        }
    }
}