using UnityEngine;

namespace GpuOptimization
{
    public class Body : MonoBehaviour
    {
        [SerializeField] private Mover _mover;
        [SerializeField] private Collider _collider;
        
        public Mover Mover => _mover;
        public Collider Collider => _collider;
        public float Radius => transform.localScale.x / 2;
        
        public void Initialize()
        {
            _mover.Direction = UnityEngine.Random.onUnitSphere;
        }

        public void Tick()
        {
            _mover.Tick();
        }
    }
}