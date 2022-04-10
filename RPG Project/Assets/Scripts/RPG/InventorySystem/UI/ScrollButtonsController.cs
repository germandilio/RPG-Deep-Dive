using UnityEngine;
using UnityEngine.UI;

namespace RPG.InventorySystem.UI
{
    [RequireComponent(typeof(Scrollbar))]
    public class ScrollButtonsController : MonoBehaviour
    {
        private Scrollbar _scrollBar;

        private void Awake()
        {
            _scrollBar = GetComponent<Scrollbar>();
        }

        public void ScrollUp()
        {
            _scrollBar.value = Mathf.Clamp01(_scrollBar.value - _scrollBar.size);
        }

        public void ScrollDown()
        {
            _scrollBar.value = Mathf.Clamp01(_scrollBar.value + _scrollBar.size);
        }
    }
}