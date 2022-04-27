using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;

namespace RPG.InventorySystem.InventoriesModel
{
    [CreateAssetMenu(menuName = "RPG Project/Drops/New Drop Library", fileName = "New DropLibrary", order = 0)]
    public class DropLibrary : ScriptableObject
    {
        [Header("Drop System Configuration")]
        [Header("Properties By Level")]
        [Tooltip("Probability of dropping anything")]
        [Range(0, 100)]
        [SerializeField]
        private int[] dropsChancePercentage;

        [Tooltip("Min-Max drops by level")]
        [MinMaxSlider(0, 100)]
        [SerializeField]
        private Vector2Int[] dropsNumber;
        
        [SerializeField]
        private DropConfig[] dropsLibrary;

        [Serializable]
        public class DropConfig
        {
            [Header("Drop Item Config")]
            public InventoryItem dropItem;

            [Header("Properties By Level")]
            [InfoBox(
                "Chance of dropping relating to other drops. (ex. Item1 with relativeChance = 1, Item2 with relativeChance = 3." +
                " Probabilities of dropping will be: for Item1 - 1/4 (25%), for Item2 - 3/4 (75%).)")]
            [Range(0, 100)]
            public float[] relativeChance;

            [MinMaxSlider(0, 10)]
            public Vector2Int[] number;

            public int GetRandomNumber(int level)
            {
                if (!dropItem.IsStackable) return 1;

                Vector2Int numberOnLevel = GetByLevel(number, level);
                return UnityEngine.Random.Range(numberOnLevel.x, numberOnLevel.y + 1);
            }
        }

        public struct DroppedSlot
        {
            public InventoryItem item;
            public int number;
        }

        public IEnumerable<DroppedSlot> GetRandomDrops(int level)
        {
            if (!ShouldDrop(level)) yield break;

            for (int i = 0; i < GetNumberOfDrops(level); i++)
            {
                var randomDrop = GetRandomItem(level);
                if (randomDrop == null) continue;

                yield return new DroppedSlot
                {
                    item = randomDrop.dropItem,
                    number = randomDrop.GetRandomNumber(level)
                };
            }
        }

        private bool ShouldDrop(int level)
        {
            return UnityEngine.Random.Range(0, 101) <= GetByLevel(dropsChancePercentage, level);
        }

        private int GetNumberOfDrops(int level)
        {
            Vector2Int numberOnLevel = GetByLevel(dropsNumber, level);
            return UnityEngine.Random.Range(numberOnLevel.x, numberOnLevel.y + 1);
        }

        private float GetTotalChance(int level)
        {
            return dropsLibrary.Select(drop => GetByLevel(drop.relativeChance, level)).Sum();
        }

        private DropConfig GetRandomItem(int level)
        {
            float totalChance = GetTotalChance(level);
            float randomRoll = UnityEngine.Random.Range(0, totalChance);

            float chanceTotal = 0f;
            foreach (var dropConfig in dropsLibrary)
            {
                chanceTotal += GetByLevel(dropConfig.relativeChance, level);
                if (chanceTotal > randomRoll)
                {
                    return dropConfig;
                }
            }

            return null;
        }

        private static T GetByLevel<T>(IReadOnlyList<T> values, int level)
        {
            if (values.Count == 0 || level <= 0) return default;
            if (level > values.Count) return values[values.Count - 1];
            return values[level];
        }
    }
}