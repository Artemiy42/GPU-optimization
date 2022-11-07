using System;
using Unity.Collections;
using UnityEngine;

namespace GpuOptimization
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private Vector2 _speedRange;
        [SerializeField, ReadOnly] private float _speed;
        
        public Vector3 Direction { get; set; }
        public float Speed { get; set; }
        
        public void StartMove()
        {
            enabled = true;
        }

        public void StopMove()
        {
            enabled = false;
        }

        public void Tick()
        {
            _speed = Speed;
            transform.position += Direction * (Speed * Time.fixedDeltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Direction * (transform.localScale.x + 0.5f));
        }

        private MoveData GetRandomMoveData()
        {
            var newDirection = UnityEngine.Random.onUnitSphere;
            var newSpeed = UnityEngine.Random.Range(_speedRange.x, _speedRange.y);
            return new MoveData
            {
                Direction = newDirection,
                Speed = newSpeed
            };
        }
    }
}
