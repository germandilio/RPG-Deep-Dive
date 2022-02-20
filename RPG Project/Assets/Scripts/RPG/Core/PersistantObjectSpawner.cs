using System;
using UnityEngine;

namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject persistentObjectPrefab;

        private static bool _wasSpawned;

        private void Awake()
        {
            if (_wasSpawned) return;

            SpawnObject();
            _wasSpawned = true;
        }

        private void SpawnObject()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}