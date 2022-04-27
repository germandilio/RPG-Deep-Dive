using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.GameplayCore.Quests.UI
{
    public class ObjectiveUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI description;

        [SerializeField]
        private Image checkboxImage;

        public void Setup(string title, bool isComplete)
        {
            if (!string.IsNullOrEmpty(title))
            {
                description.text = title;
                checkboxImage.enabled = isComplete;
            }
        }
    }
}