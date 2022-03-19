using System;
using UnityEngine;

namespace RPG.UI
{
    public class UICameraFacing : MonoBehaviour
    {
        private void Update()
        {
            if (Camera.main == null) return;

            transform.forward = Camera.main.transform.forward;
        }
    }
}