using UnityEngine;
using UnityEngine.UI;

namespace Utils.UI
{
    /// <summary>
    /// Generic blueprint for displayBars
    /// </summary>
    /// <typeparam name="T">Type of component which values should be displayed, this component must be attached to GameObject tagged "Player"</typeparam>
    [RequireComponent(typeof(Slider))]
    public abstract class DisplayBar<T> : MonoBehaviour where T : MonoBehaviour
    {
        private Slider _displayBar;
        
        protected T playerStatComponent;

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            playerStatComponent = player.GetComponent<T>();

            _displayBar = GetComponent<Slider>();
        }

        private void Update()
        {
            if (playerStatComponent == null)
            {
                Debug.LogError($"Players component {nameof(T)} is null");
            }

            _displayBar.value = GetCurrentValue() / GetMaxValue();
        }

        protected abstract float GetCurrentValue();

        protected abstract float GetMaxValue();
    }
}