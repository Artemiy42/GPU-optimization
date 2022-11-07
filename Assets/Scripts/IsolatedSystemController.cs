using UniRx;
using UnityEngine;

namespace GpuOptimization
{
    public class IsolatedSystemController : MonoBehaviour
    {
        [SerializeField] private Transform _bodyParent;
        [SerializeField] private Borders _borders;
        [SerializeField] private MainUi mainUi;
        
        private GameFactory _gameFactory;
        private IsolatedSystem _isolatedSystem;
        private CompositeDisposable _disposable;

        private void Start()
        {
            _gameFactory = new GameFactory();
            _isolatedSystem = new IsolatedSystem(_gameFactory, _borders, _bodyParent);
            _isolatedSystem.Initialize();
        }

        private void OnEnable()
        {
            _disposable = new();
            mainUi.OnPlusButtonClicked.Subscribe(PlusButtonHandler).AddTo(_disposable);
            mainUi.OnMinusButtonClicked.Subscribe(MinusButtonHandler).AddTo(_disposable);
            mainUi.OnSpeedChange.Subscribe(SpeedChangeHandler).AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable.Dispose();
            _disposable = new CompositeDisposable();
        }

        private void FixedUpdate()
        {
            // _isolatedSystem.Tick();   
        }
        
        private void PlusButtonHandler(int value)
        {
            _isolatedSystem.CreateBodies(value);
            mainUi.SetAmountOfBodies(_isolatedSystem.AmountOfBodies);
        }

        private void MinusButtonHandler(int value)
        {
            _isolatedSystem.DeleteBodies(value);
            mainUi.SetAmountOfBodies(_isolatedSystem.AmountOfBodies);
        }
        
        private void SpeedChangeHandler(float speed)
        {
            _isolatedSystem.ChangeSpeed(speed);
        }
    }
}