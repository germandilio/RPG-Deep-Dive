using TMPro;
using UnityEngine;

namespace Utils.UI.Hint
{
    public class HintController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI hintDescription;

        public void SetDescription(string description)
        {
            if (hintDescription != null && !string.IsNullOrEmpty(description))
                hintDescription.text = description;
        }
    }
}