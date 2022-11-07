using System;
using System.Collections.Generic;
using UnityEngine;

namespace GpuOptimization
{
    public class IsolatedSystem
    {
        private const float MaxSpeed = 10;
        private const int MaxAmountBodies = 10000;

        private readonly GameFactory _gameFactory;
        private readonly Borders _borders;
        private readonly Transform _bodyParent;
        private readonly List<Body> _bodies = new();
        private readonly Plane[] _planes = new Plane[6];
        private readonly Collider[] _colliders = new Collider[2];

        private float _currentSpeed = 0;
        
        public int AmountOfBodies => _bodies.Count;
        
        public IsolatedSystem(GameFactory gameFactory, Borders borders, Transform bodyParent)
        {
            _gameFactory = gameFactory;
            _borders = borders;
            _bodyParent = bodyParent;
        }

        public void Initialize()
        {
            InitializeBorders();
        }

        private void InitializeBorders()
        {
            var bounds = _borders.Bounds;
            _planes[0] = new Plane(Vector3.left, bounds.center + Vector3.right * bounds.extents.x);
            _planes[1] = new Plane(Vector3.right, bounds.center + Vector3.left * bounds.extents.x);
            _planes[2] = new Plane(Vector3.up, bounds.center + Vector3.down * bounds.extents.y);
            _planes[3] = new Plane(Vector3.down, bounds.center + Vector3.up * bounds.extents.y);
            _planes[4] = new Plane(Vector3.back, bounds.center + Vector3.forward * bounds.extents.z);
            _planes[5] = new Plane(Vector3.forward, bounds.center + Vector3.back * bounds.extents.z);
        }

        public void CreateBody()
        {
            if (_bodies.Count + 1 > MaxAmountBodies)
                throw new Exception($"Amount of bodies need to be less then {MaxAmountBodies}");

            var body = _gameFactory.CreateBody(_bodyParent);
            body.Initialize();
            body.Mover.Speed = _currentSpeed;

            PlaceBodyInEmptySpace(body);

            _bodies.Add(body);
        }

        public void CreateBodies(int amountBodies)
        {
            if (amountBodies <= 0)
                throw new ArgumentException("Need to be greater then 0", nameof(amountBodies));

            for (int i = 0; i < amountBodies; i++)
            {
                CreateBody();
            }
        }

        public void DeleteBody()
        {
            if (_bodies.Count == 0)
            {
                Debug.Log("IsolatedSystem has 0 body!");
                return;
            }

            _gameFactory.ReleaseBody(_bodies[^0]);
            _bodies.RemoveAt(_bodies.Count - 1);
        }

        public void DeleteBodies(int amountOfBodies)
        {
            if (amountOfBodies <= 0)
                throw new ArgumentException("need to be greater then 0", nameof(amountOfBodies));

            if (_bodies.Count < amountOfBodies)
                throw new Exception($"IsolatedSystem has {_bodies.Count} bodies then need to delete {amountOfBodies} bodies!");

            for (int i = 0; i < amountOfBodies; i++)
            {
                _gameFactory.ReleaseBody(_bodies[^(i + 1)]);
            }
            _bodies.RemoveRange(_bodies.Count - amountOfBodies, amountOfBodies);
        }

        public void ChangeSpeed(float speed)
        {
            _currentSpeed = speed * MaxSpeed;
            foreach (var body in _bodies)
            {
                body.Mover.Speed = _currentSpeed;
            }
        }

        public void RunBodies()
        {
            foreach (var body in _bodies)
            {
                body.Mover.StartMove();
            }
        }

        public void StopBodies()
        {
            foreach (var body in _bodies)
            {
                body.Mover.StopMove();
            }
        }

        public void Tick()
        {
            CheckCollisions();
            UpdateBodies();
        }

        private void CheckCollisions()
        {
            foreach (var body in _bodies)
            {
                var amountCollisions = Physics.OverlapSphereNonAlloc(body.transform.position, body.transform.localScale.x / 2, _colliders, LayerMask.GetMask("Body"));
                var bodyMover = body.Mover;
                
                if (amountCollisions > 1)
                {
                    var directionToCollider = bodyMover.transform.position - _colliders[1].transform.position;
                    var normalizedDirectionToCollider = directionToCollider.normalized;
                    var reflectDirection = Vector3.Reflect(bodyMover.Direction, normalizedDirectionToCollider).normalized;
                    
                    // bodyMover.transform.position += -bodyMover.Direction * (directionToCollider.magnitude - body.Radius); 
                    bodyMover.Direction = reflectDirection;
                    continue;
                }

                CheckCollisionWithBorders(body);
            }   
        }

        private void CheckCollisionWithBorders(Body body)
        {
            foreach (var plane in _planes)
            {
                var distanceToPoint = plane.GetDistanceToPoint(body.transform.position);
                if (distanceToPoint < body.Radius)
                {
                    var extraDistance = distanceToPoint;
                    var dotPlaneNormalToBodyDirection = Vector3.Dot(plane.normal, body.Mover.Direction);
                    if (dotPlaneNormalToBodyDirection < 0)
                    {
                        body.Mover.Direction = Vector3.Reflect(body.Mover.Direction, plane.normal).normalized;
                    }

                    var backDirection = plane.normal * Mathf.Abs(extraDistance);
                    body.transform.position += backDirection;
                    // Debug.Log($" dir: {backDirection} distanceToPoint: {distanceToPoint}");
                }
            }
        }

        private void UpdateBodies()
        {
            foreach (var body in _bodies)
            {
                body.Tick();
            }
        }

        private void PlaceBodyInEmptySpace(Body body)
        {
            var bounds = _borders.Bounds;
            bounds.size -= Vector3.one * (body.Radius + 1f);
            body.transform.position = Random.RandomPointInBounds(bounds);
        }
    }
}