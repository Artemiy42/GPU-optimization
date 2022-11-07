using UnityEngine;

namespace GpuOptimization
{
    public class GameFactory
    {
        private static class PrefabPath
        {
            public const string Body = "Body";
        }
        
        public Body CreateBody(Transform parent = null)
        {
            var prefab = Resources.Load<Body>(PrefabPath.Body);
            return Object.Instantiate(prefab, parent);
        }

        public void ReleaseBody(Body body)
        {
            Object.Destroy(body.gameObject);
        }
    }
}