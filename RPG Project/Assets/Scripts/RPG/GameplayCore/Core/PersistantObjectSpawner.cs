using UnityEngine;

namespace RPG.GameplayCore.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("This prefab will only be instantiated once and persisted between scenes.")]
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