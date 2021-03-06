using UnityEngine;

namespace RPG.GameplayCore.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField]
        private float waypointGizmoRadius = 0.5f;

        private int _currentIndex;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);

                int nextIndex = NextIndex(i);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(nextIndex));
            }
        }
#endif

        public Vector3 GetCurrentWaypoint() => GetWaypoint(_currentIndex);

        private int NextIndex(int index) => (index + 1) % transform.childCount;

        /// <summary>
        /// </summary>
        /// <param name="index">Index of child in path</param>
        /// <returns>Position of waypoint in world space coordinates</returns>
        private Vector3 GetWaypoint(int index) => transform.GetChild(index).position;

        public void CycleWaypoint()
        {
            _currentIndex = NextIndex(_currentIndex);
        }
    }
}