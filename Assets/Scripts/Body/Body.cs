using UnityEngine;

namespace GpuOptimization
{
    public class Body : MonoBehaviour
    {
        [SerializeField] private Mover _mover;
        [SerializeField] private View _view;
        [SerializeField] private Collider _collider;
        
        public Mover Mover => _mover;
        public Collider Collider => _collider;
        public View View => _view;

        public float Radius => transform.localScale.x / 2;
        public float SqrRadius => transform.localScale.x;
        
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