using RPG.GameplayCore.Attributes;
using UnityEngine;

namespace RPG.InventorySystem.InventoriesModel.Actions
{
    [CreateAssetMenu(menuName = "RPG Project/Inventory/New HealActionItem", fileName = "New HealActionItem", order = 4)]
    public class HealActionItem : ActionItem
    {
        [Header("Heal Properties")]
        [SerializeField]
        private int healthPointsToRestore = 50;

        public override bool Use(GameObject user)
        {
            if (user == null) return false;

            Health health = user.GetComponent<Health>();
            bool wasHealed = health.Heal(healthPointsToRestore);

            if (wasHealed)
                health.ShowHealEffect();

            return wasHealed;
        }
    }
}