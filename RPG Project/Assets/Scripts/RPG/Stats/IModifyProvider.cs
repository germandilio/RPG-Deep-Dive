using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IModifyProvider
    {
        IEnumerable<float> GetAdditiveModifier(Stats stats);

        IEnumerable<float> GetPercentageModifier(Stats stats);
    }
}