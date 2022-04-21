using UnityEngine;
using UnityEngine.UI;

namespace RPG.GameplayCore.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [Header("Panel Properties")]
        [SerializeField]
        private GameObject uiContainer;

        [SerializeField]
        private Button uiButton;

        [SerializeField]
        private KeyCode toggleKey = KeyCode.None;

        #region Validation Editor code

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (uiContainer != null && uiButton == null)
                Debug.LogError($"Setup button for panel menu: {uiContainer}");
        }
#endif

        #endregion

        void Start()
        {
            uiContainer.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
                SwitchState();
        }

        public void SwitchState()
        {
            if (uiContainer != null)
                uiContainer.SetActive(!uiContainer.activeSelf);
        }
    }
}