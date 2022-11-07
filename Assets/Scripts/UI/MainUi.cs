using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GpuOptimization
{
    public class MainUi : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountOfBodiesText;
        [SerializeField] private Button _plus1Button;
        [SerializeField] private Button _minus1Button;
        [SerializeField] private Button _plus10Button;
        [SerializeField] private Button _minus10Button;
        [SerializeField] private Slider _slider;
        
        private CompositeDisposable _disposables;
        private ISubject<Unit> _onPlus1ButtonClicked = new Subject<Unit>();
        private ISubject<Unit> _onPlus10ButtonClicked = new Subject<Unit>();
        private ISubject<Unit> _onMinus1ButtonClicked = new Subject<Unit>();
        private ISubject<Unit> _onMinus10ButtonClicked = new Subject<Unit>();
        private ISubject<float> _onSpeedChange = new Subject<float>();

        public IObservable<Unit> OnPlus1ButtonClicked => _onPlus1ButtonClicked;
        public IObservable<Unit> OnPlus10ButtonClicked => _onPlus10ButtonClicked;
        public IObservable<Unit> OnMinusButtonClicked => _onMinus1ButtonClicked;
        public IObservable<Unit> OnMinus10ButtonClicked => _onMinus10ButtonClicked;
        public IObservable<float> OnSpeedChange => _onSpeedChange;
        
        public void SetAmountOfBodies(int amountOfBodies)
        {
            _amountOfBodiesText.text = amountOfBodies.ToString();
        }

        private void OnEnable()
        {
            _disposables = new CompositeDisposable();
            _plus1Button.OnClickAsObservable().Subscribe(_ => Plus1ButtonHandler()).AddTo(_disposables);
            _plus10Button.OnClickAsObservable().Subscribe(_ => Plus10ButtonHandler()).AddTo(_disposables);
            _minus1Button.OnClickAsObservable().Subscribe(_ => Minus1ButtonHandler()).AddTo(_disposables);
            _minus10Button.OnClickAsObservable().Subscribe(_ => Minus10ButtonHandler()).AddTo(_disposables);
            _slider.onValueChanged.AsObservable().Subscribe(_ => OnSliderChangeHandler()).AddTo(_disposables);
        }

        private void OnSliderChangeHandler()
        {
            _onSpeedChange.OnNext(_slider.value);
        }

        private void OnDisable()
        {
            _disposables.Dispose();
        }

        private void Plus1ButtonHandler()
        {
            _onPlus1ButtonClicked.OnNext(Unit.Default);
        }

        private void Plus10ButtonHandler()
        {
            _onPlus10ButtonClicked.OnNext(Unit.Default);
        }

        private void Minus1ButtonHandler()
        {
            _onMinus1ButtonClicked.OnNext(Unit.Default);
        }

        private void Minus10ButtonHandler()
        {
            _onMinus10ButtonClicked.OnNext(Unit.Default);
        }
    }
}