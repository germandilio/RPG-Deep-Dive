using System;
using System.Collections.Generic;
using NaughtyAttributes;
using RPG.GameplayCore.Quests.QuestsModel;
using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;

namespace RPG.GameplayCore.Quests.Rewards
{
    /// <summary>
    /// List of items, that will be removed from player inventory.
    /// </summary>
    [Serializable]
    public class ObjectiveCompletionWithdrawals
    {
        [Required]
        [Tooltip("Objective, on which completion items will be removed from player inventory.")]
        public Objective objective;

        public List<InventorySlot> withdrawals;
    }
}