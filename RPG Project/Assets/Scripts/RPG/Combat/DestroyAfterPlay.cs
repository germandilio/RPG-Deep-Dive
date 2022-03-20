using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyAfterPlay : MonoBehaviour, IDestroyer
    {
        [SerializeField]
        private GameObject targetToDestroy;

        private ParticleSystem _effectSystem;

        private void Awake()
        {
            _effectSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (!_effectSystem.IsAlive())
            {
                DestroyTarget();
            }
        }

        public void DestroyTarget()
        {
            Destroy(targetToDestroy != null ? targetToDestroy : gameObject);
        }
    }
}