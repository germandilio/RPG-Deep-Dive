using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField]
        private float waypointGizmoRadius = 0.5f;

        private int _currentIndex = 0;
        
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);

                int nextIndex = NextIndex(i);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(nextIndex));
            }
        }

        public Vector3 GetCurrentWaypoint() => GetWaypoint(_currentIndex);
        
        private int NextIndex(int index) => (index + 1) % transform.childCount;

        /// <summary>
        /// 
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
