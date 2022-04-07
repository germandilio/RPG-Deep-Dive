using UnityEngine;

namespace SavingSystem
{
    [System.Serializable]
    public class SerializableVector3
    {
        private float x, y, z;

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector()
        {
            return new Vector3(x, y, z);
        }
    }
}