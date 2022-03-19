using System;
using UnityEngine;
using UnityEngine.AI;

namespace Utils
{
    public class NavMeshExtensions
    {
        public static float CalculateLength(NavMeshPath path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            
            float length = 0f;
            if (path.corners.Length < 2) return length;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                length += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return length;
        }
    }
}