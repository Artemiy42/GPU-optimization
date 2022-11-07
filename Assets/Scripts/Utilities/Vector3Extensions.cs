using UnityEngine;

namespace GpuOptimization
{
    public static class Vector3Extensions
    {
        public static Vector3 Round(this Vector3 vector)
        {
            return new Vector3
            {
                x = Mathf.Round(vector.x),
                y = Mathf.Round(vector.y),
                z = Mathf.Round(vector.z),
            };
        }
    }
}