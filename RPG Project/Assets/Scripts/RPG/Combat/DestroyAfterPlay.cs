using System;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyAfterPlay : MonoBehaviour
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
                if (targetToDestroy != null)
                    Destroy(targetToDestroy);
                else
                    Destroy(gameObject);
            }
        }
    }
}