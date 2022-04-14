using UnityEngine;

namespace RPG.GameplayCore.Attributes
{
    public class InGameHealthBar : MonoBehaviour
    {
        [SerializeField]
        private RectTransform barTransform;

        [SerializeField]
        private Health enemyHealth;

        [SerializeField]
        private Canvas canvas;

        private void Update()
        {
            float fraction = enemyHealth.GetCurrentHealth() / enemyHealth.GetMaxHealth();
            if (Mathf.Approximately(fraction, 0f) || Mathf.Approximately(fraction, 1f))
            {
                canvas.enabled = false;
                return;
            }

            canvas.enabled = true;
            barTransform.localScale = new Vector3(fraction, 1, 1);
        }
    }
}