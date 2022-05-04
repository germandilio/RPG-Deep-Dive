using RPG.InventorySystem.InventoriesModel.Inventory;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.InventorySystem.UI.Inventories
{
    /// <summary>
    /// To be put on the icon representing an inventory item. Allows the slot to
    /// update the icon and number.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        [SerializeField]
        private GameObject textContainer;

        [SerializeField]
        private TextMeshProUGUI itemNumber;

        private Sprite _iconWithoutBackground;
        private Sprite _iconWithBackground;

        public void SetItem(InventoryItem item)
        {
            SetItem(item, 1);
        }

        public void SetItem(InventoryItem item, int number)
        {
            var iconImage = GetComponent<Image>();
            if (item == null)
            {
                iconImage.enabled = false;
                _iconWithoutBackground = null;
                _iconWithBackground = null;
            }
            else
            {
                iconImage.enabled = true;
                _iconWithoutBackground = item.Icon;
                _iconWithBackground = item.IconWithBackground;
                iconImage.sprite = _iconWithBackground;
            }

            if (itemNumber)
            {
                if (number <= 1)
                    textContainer.SetActive(false);
                else
                {
                    textContainer.SetActive(true);
                    itemNumber.text = number.ToString();
                }
            }
        }

        public void CutOffBackground()
        {
            if (_iconWithoutBackground != null)
            { 
                var iconImage = GetComponent<Image>(); 
                iconImage.sprite = _iconWithoutBackground;
            }
        }

        public void ShowBackground()
        {
            if (_iconWithBackground != null)
            { 
                var iconImage = GetComponent<Image>(); 
                iconImage.sprite = _iconWithBackground;
            }
        }
    }
}