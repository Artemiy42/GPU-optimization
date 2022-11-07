using UnityEngine;

namespace GpuOptimization
{
    public class Borders : MonoBehaviour
    {
        [SerializeField] private Collider _border;

        public Bounds Bounds => _border.bounds;
    }
}