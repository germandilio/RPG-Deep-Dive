using Utils.UI;

namespace RPG.GameplayCore.Attributes
{
    public class HealthDisplay : DisplayBar<Health>
    {
        protected override float GetCurrentValue()
        {
            return characterStatComponent.GetCurrentHealth();
        }

        protected override float GetMaxValue()
        {
            return characterStatComponent.GetMaxHealth();
        }

        protected override bool ShouldShow()
        {
            return true;
        }
    }
}