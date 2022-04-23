using System;
using System.Collections.Generic;
using RPG.GameplayCore.Quests.QuestsModel;
using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;

namespace RPG.GameplayCore.Quests.Rewards
{
    /// <summary>
    /// List of items, which player will receive on objective completion.
    /// </summary>
    [Serializable]
    public class ObjectiveCompletionReward
    {
        [Tooltip("Objective, on which completion player will receive item.")]
        public Objective objective;
        
        public List<InventorySlot> rewards;
    }
}