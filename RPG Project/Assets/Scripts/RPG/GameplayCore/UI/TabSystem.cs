using System;
using UnityEngine;

namespace RPG.GameplayCore.UI
{
    public class TabSystem : MonoBehaviour
    {
        public event Action<GameObject> TabSelected;

        private void Start()
        {
            TabSelected?.Invoke(null);
        }

        public void Select(GameObject uiContainer)
        {
            uiContainer.SetActive(true);
            TabSelected?.Invoke(uiContainer);
        }
    }
}