using UnityEngine;

namespace RPG.Core
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