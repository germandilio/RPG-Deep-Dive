using UnityEngine;

namespace Utils
{
    public static class RaycastUtils
    {
        /// <summary>
        /// Convert mouse position to ray from camera to object under mouse pointer
        /// </summary>
        /// <returns>Ray, where origin: main camera.</returns>
        public static Ray GetMouseRay()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Debug.DrawRay(ray.origin, ray.direction * 1000);
            return ray;
        }
    }
}