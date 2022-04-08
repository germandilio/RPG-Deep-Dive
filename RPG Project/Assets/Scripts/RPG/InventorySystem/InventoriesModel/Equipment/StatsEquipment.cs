using System.Collections.Generic;
using RPG.GameplayCore.Stats;

namespace RPG.InventorySystem.InventoriesModel.Equipment
{
    /// <summary>
    /// Equipment that provides effect on player stats.
    /// </summary>
    public class StatsEquipment : Equipment, IModifyProvider
    {
        IEnumerable<float> IModifyProvider.GetAdditiveModifier(Stats statsType)
        {
            foreach (var equipLocation in GetAllEquipLocations())
            {
                if (GetItemInSlot(equipLocation) is IModifyProvider equipItem)
                {
                    foreach (var modifier in equipItem.GetAdditiveModifier(statsType))
                    {
                        yield return modifier;
                    }
                }
            }
        }

        IEnumerable<float> IModifyProvider.GetPercentageModifier(Stats statsType)
        {
            foreach (var equipLocation in GetAllEquipLocations())
            {
                if (GetItemInSlot(equipLocation) is IModifyProvider equipItem)
                {
                    foreach (var modifier in equipItem.GetPercentageModifier(statsType))
                    {
                        yield return modifier;
                    }
                }
            }
        }
    }
}