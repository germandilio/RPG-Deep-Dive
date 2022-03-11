using System;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyAfterPlay : MonoBehaviour
    {
        private ParticleSystem _effectSystem;

        private void Awake()
        {
            _effectSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (!_effectSystem.IsAlive()) Destroy(gameObject);
        }
    }
}