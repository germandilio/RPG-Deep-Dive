using UnityEngine;

namespace RPG.GameplayCore.UI
{
    public class UICameraFacing : MonoBehaviour
    {
        private void LateUpdate()
        {
            if (Camera.main == null) return;

            transform.forward = Camera.main.transform.forward;
        }
    }
}