using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;
using TMPro;

namespace RPG.InventorySystem.UI.Inventories
{
    /// <summary>
    /// Root of the tooltip prefab to expose properties to other classes.
    /// </summary>
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private TextMeshProUGUI bodyText;

        public void Setup(InventoryItem item)
        {
            titleText.text = item.DisplayName;
            bodyText.text = item.Description;
        }
    }
}