using Utils.UI;

namespace RPG.GameplayCore.Attributes
{
    public class HealthDisplay : DisplayBar<Health>
    {
        protected override float GetCurrentValue()
        {
            return playerStatComponent.GetCurrentHealth();
        }

        protected override float GetMaxValue()
        {
            return playerStatComponent.GetMaxHealth();
        }
    }
}