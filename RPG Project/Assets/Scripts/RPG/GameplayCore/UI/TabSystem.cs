using System;
using UnityEngine;

namespace RPG.GameplayCore.UI
{
    public class TabSystem : MonoBehaviour
    {
        public event Action TabSelected;

        public GameObject SelectedTab { get; private set; }

        private void Start()
        {
            TabSelected?.Invoke();
        }

        public void Select(GameObject uiContainer)
        {
            SelectedTab = uiContainer;
            TabSelected?.Invoke();
        }
    }
}