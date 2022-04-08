using UnityEngine;

namespace RPG.InventorySystem.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField]
        private KeyCode toggleKey = KeyCode.Escape;

        [SerializeField]
        private GameObject uiContainer;

        void Start()
        {
            uiContainer.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(toggleKey) || Input.GetKeyDown(KeyCode.Escape))
                uiContainer.SetActive(!uiContainer.activeSelf);
        }
    }
}