using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.GameplayCore.UI
{
    [RequireComponent(typeof(Button))]
    public class TabButton : MonoBehaviour
    {
        [Label("Tab")]
        [SerializeField]
        private GameObject uiContainer;
        
        [SerializeField]
        private TabSystem tabSystem;

        [SerializeField]
        private KeyCode toggleKey = KeyCode.None;

        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            if (_button != null)
                _button.onClick.AddListener(SwitchState);

            tabSystem.TabSelected += ResetTab;
        }

        private void ResetTab(GameObject uiTab)
        {
            if (!ReferenceEquals(uiTab, uiContainer))
            {
                uiContainer.SetActive(false);
            }
        }

        private void OnDisable()
        {
            tabSystem.TabSelected -= ResetTab;
        }

        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
                SwitchState();
        }

        [Button("Show/Hide")]
        public void SwitchState() => tabSystem.Select(uiContainer);
    }
}