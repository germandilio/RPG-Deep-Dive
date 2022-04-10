using RPG.GameplayCore.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.InventorySystem.InventoriesModel
{
    [RequireComponent(typeof(BaseStats))]
    public class RandomDropper : ItemDropper
    {
        [Tooltip("How far drops can be spawned from the dropper")]
        [SerializeField]
        private float spawnDistance = 1f;

        [SerializeField]
        private DropLibrary dropsLibrary;

        [SerializeField]
        private float spawningDelay = 0.4f;

        private const int Attempts = 10;

        protected override Vector3 GetLocationToDrop()
        {
            for (int i = 0; i < Attempts; i++)
            {
                Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnDistance;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPosition, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            return transform.position;
        }

        /// <summary>
        /// Drops random item from dropsLibrary
        /// </summary>
        /// <remarks>
        /// Event function.
        /// </remarks>
        public void RandomDrop()
        {
            if (dropsLibrary == null)
            {
                Debug.LogError("Drops Library is not set");
                return;
            }
            
            Invoke(nameof(Drop), spawningDelay);
        }

        private void Drop()
        {
            int level = GetComponent<BaseStats>().GetLevel();
            foreach (var droppedSlot in dropsLibrary.GetRandomDrops(level))
            {
                DropItem(droppedSlot.item, droppedSlot.number);
            }
        }
    }
}