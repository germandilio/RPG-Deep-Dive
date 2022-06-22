using UnityEngine;
using UnityEngine.UI;

namespace Utils.UI
{
    /// <summary>
    /// Generic blueprint for displayBars
    /// </summary>
    /// <typeparam name="T">Type of component which values should be displayed, this component must be attached to GameObject tagged "Player"</typeparam>
    public abstract class DisplayBar<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private Slider displayBar;

        protected T characterStatComponent;

        protected float? Fraction
        {
            get
            {
                if (displayBar == null)
                    return null;

                return displayBar.value;
            }
        }

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            characterStatComponent = player.GetComponent<T>();
        }

        private void Update()
        {
            if (characterStatComponent == null)
            {
                Debug.LogError($"Players component {nameof(T)} is null");
            }

            if (canvas != null)
            {
                canvas.enabled = ShouldShow();
            }

            displayBar.value = GetCurrentValue() / GetMaxValue();
        }

        protected abstract float GetCurrentValue();

        protected abstract float GetMaxValue();

        protected abstract bool ShouldShow();
    }
}