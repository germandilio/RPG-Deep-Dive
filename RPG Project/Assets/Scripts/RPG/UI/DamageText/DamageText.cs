using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField]
        private Text textComponent;

        public void SetText(float damage)
        {
            textComponent.text = $"{damage:0}";
        }
    }
}