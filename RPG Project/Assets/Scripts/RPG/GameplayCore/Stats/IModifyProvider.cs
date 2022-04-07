using System.Collections.Generic;

namespace RPG.GameplayCore.Stats
{
    public interface IModifyProvider
    {
        IEnumerable<float> GetAdditiveModifier(Stats stats);

        IEnumerable<float> GetPercentageModifier(Stats stats);
    }
}