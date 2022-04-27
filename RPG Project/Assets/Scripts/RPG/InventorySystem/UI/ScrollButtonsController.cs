using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.InventorySystem.UI
{
    [RequireComponent(typeof(Scrollbar))]
    public class ScrollButtonsController : MonoBehaviour
    {
        [InfoBox("1 - opening on the inventory top, 0 - on the bottom")]
        [Range(0, 1)]
        [SerializeField]
        private float initScrollbarPosition = 1f;

        private Scrollbar _scrollBar;

        private void Awake()
        {
            _scrollBar = GetComponent<Scrollbar>();
        }

        private void Start()
        {
            _scrollBar.value = initScrollbarPosition;
        }

        [Button("Scroll up")]
        public void ScrollUp()
        {
            _scrollBar.value = Mathf.Clamp01(_scrollBar.value - _scrollBar.size);
        }

        [Button("Scroll down")]
        public void ScrollDown()
        {
            _scrollBar.value = Mathf.Clamp01(_scrollBar.value + _scrollBar.size);
        }
    }
}