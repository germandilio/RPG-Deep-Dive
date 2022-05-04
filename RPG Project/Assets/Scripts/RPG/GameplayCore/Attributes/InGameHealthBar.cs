using UnityEngine;
using Utils.UI;

namespace RPG.GameplayCore.Attributes
{
    public class InGameHealthBar : DisplayBar<Health>
    {
        [SerializeField]
        private Health enemyHealth;

        protected override float GetCurrentValue()
        {
            return enemyHealth.GetCurrentHealth();
        }

        protected override float GetMaxValue()
        {
            return enemyHealth.GetMaxHealth();
        }

        protected override bool ShouldShow()
        {
            if (!Fraction.HasValue) return false;
            return !Mathf.Approximately(Fraction.Value, 0f) && !Mathf.Approximately(Fraction.Value, 1f);
        }
    }
}