using System;
using System.Collections.Generic;
using System.Linq;
using RPG.GameplayCore.Stats;
using UnityEngine;

namespace RPG.InventorySystem.InventoriesModel.Equipment
{
    [CreateAssetMenu(menuName = "RPG Project/Inventory/New StatsEquippableItem", fileName = "New StatsEquippableItem",
        order = 1)]
    public class StatsEquippableItem : EquippableItem, IModifyProvider
    {
        [Header("Stats Modifiers")]
        [SerializeField]
        private Modifier[] additiveModifiers;

        [SerializeField]
        private Modifier[] percentageModifiers;

        [Serializable]
        private struct Modifier
        {
            public Stats stat;
            public float value;
        }

        IEnumerable<float> IModifyProvider.GetAdditiveModifier(Stats statsType)
        {
            return GetModifiersOfType(additiveModifiers, statsType);
        }

        IEnumerable<float> IModifyProvider.GetPercentageModifier(Stats statsType)
        {
            return GetModifiersOfType(percentageModifiers, statsType);
        }

        private IEnumerable<float> GetModifiersOfType(Modifier[] modifiers, Stats statsType)
        {
            return modifiers
                .Where(modifier => modifier.stat == statsType)
                .Select(modifier => modifier.value);
        }
    }
}