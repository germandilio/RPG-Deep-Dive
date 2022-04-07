using UnityEngine;

namespace RPG.GameplayCore.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        private void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}