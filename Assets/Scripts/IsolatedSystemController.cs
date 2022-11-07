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
            mainUi.OnPlus1ButtonClicked.Subscribe(PlusButtonHandler).AddTo(_disposable);
            mainUi.OnPlus10ButtonClicked.Subscribe(Plus10ButtonHandler).AddTo(_disposable);
            mainUi.OnMinusButtonClicked.Subscribe(MinusButtonHandler).AddTo(_disposable);
            mainUi.OnMinus10ButtonClicked.Subscribe(Minus10ButtonHandler).AddTo(_disposable);
            mainUi.OnSpeedChange.Subscribe(SpeedChangeHandler).AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable.Dispose();
            _disposable = new CompositeDisposable();
        }

        private void FixedUpdate()
        {
            _isolatedSystem.Tick();   
        }
        
        private void PlusButtonHandler(Unit _)
        {
            _isolatedSystem.CreateBody();
            mainUi.SetAmountOfBodies(_isolatedSystem.AmountOfBodies);
        }

        private void Plus10ButtonHandler(Unit _)
        {
            _isolatedSystem.CreateBodies(10);
            mainUi.SetAmountOfBodies(_isolatedSystem.AmountOfBodies);
        }

        private void MinusButtonHandler(Unit _)
        {
            _isolatedSystem.DeleteBody();
            mainUi.SetAmountOfBodies(_isolatedSystem.AmountOfBodies);
        }

        private void Minus10ButtonHandler(Unit _)
        {
            _isolatedSystem.DeleteBodies(10);
            mainUi.SetAmountOfBodies(_isolatedSystem.AmountOfBodies);
        }

        private void SpeedChangeHandler(float speed)
        {
            _isolatedSystem.ChangeSpeed(speed);
        }
    }
}