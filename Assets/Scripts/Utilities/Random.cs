using UnityEngine;

namespace GpuOptimization
{
    public static class Random
    {
        public static Vector3 RandomPointInBounds(Bounds bounds) {
            return new Vector3(
                UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
                UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
                UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
            );
        }
    }
}