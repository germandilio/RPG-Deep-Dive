using RPG.Core;
using UnityEngine;

namespace RPG.UI
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