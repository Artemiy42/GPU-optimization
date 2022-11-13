using UnityEngine;

namespace GpuOptimization
{
    public class View : MonoBehaviour
    {
        private const string MainColorPropertyName = "_MainColor";
        private static MaterialPropertyBlock _materialPropertyBlock;
        
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material _material;

        public void Start()
        {
            _meshRenderer.material = _material;
            _materialPropertyBlock ??= new MaterialPropertyBlock();
        }
        
        public void SetRandomColor()
        {
            _materialPropertyBlock.Clear();
            _materialPropertyBlock.SetColor(MainColorPropertyName, UnityEngine.Random.ColorHSV());
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}