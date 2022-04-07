using RPG.GameplayCore.Core;
using UnityEngine;

namespace RPG.GameplayCore.UI
{
    public class Destroyer : MonoBehaviour, IDestroyer
    {
        [SerializeField]
        private GameObject targetToDestroy;

        public void DestroyTarget()
        {
            Destroy(targetToDestroy == null ? gameObject : targetToDestroy);
        }
    }
}