using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;
using Random = UnityEngine.Random;

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
        
        [SerializeField]
        private int[] minDropsNumber;
        [SerializeField]
        private int[] maxDropsNumber;

        [SerializeField]
        private DropConfig[] dropsLibrary;

        [Serializable]
        public class DropConfig
        {
            [Header("Drop Item Config")]
            public InventoryItem dropItem;
            
            [Header("Properties By Level")]
            [Tooltip("Chance of dropping relating to other drops. (ex. Item1 with relativeChance = 1, Item2 with relativeChance = 3." +
                     " Probabilities of dropping will be: for Item1 - 1/4 (25%), for Item2 - 3/4 (75%).)")]
            [Range(0, 100)]
            public float[] relativeChance;

            public int[] minNumber;
            public int[] maxNumber;

            public int GetRandomNumber(int level)
            {
                if (!dropItem.IsStackable) return 1;

                int min = GetByLevel(minNumber, level);
                int max = GetByLevel(maxNumber, level);
                return Random.Range(min, max + 1);
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
            return Random.Range(0, 101) <= GetByLevel(dropsChancePercentage, level);
        }

        private int GetNumberOfDrops(int level)
        {
            int min = GetByLevel(minDropsNumber, level);
            int max = GetByLevel(maxDropsNumber, level);
            
            return Random.Range(min, max + 1);
        }

        private float GetTotalChance(int level)
        {
            return dropsLibrary.Select(drop => GetByLevel(drop.relativeChance, level)).Sum();
        }

        private DropConfig GetRandomItem(int level)
        {
            float totalChance = GetTotalChance(level);
            float randomRoll = Random.Range(0, totalChance);
            
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

        private static T GetByLevel<T>(T[] values, int level)
        {
            if (values.Length == 0 || level <= 0) return default;
            if (level > values.Length) return values[values.Length - 1];
            return values[level];
        }
    }
}