using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        public void SetText(float damage)
        {
            var textLabel = GetComponent<Text>();
            textLabel.text = damage.ToString(CultureInfo.CurrentCulture);
        }
    }
}