using System;
using UnityEngine;

namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject persistentObjectPrefab;

        private bool wasSpawned;
        private void Awake()
        {
            if (wasSpawned) return;
            
            SpawnObject();
            wasSpawned = true;
        }

        private void SpawnObject()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}