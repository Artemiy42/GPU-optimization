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
        [SerializeField] private Button _plus100Button;
        [SerializeField] private Button _minus100Button;
        [SerializeField] private Slider _slider;
        
        private CompositeDisposable _disposables;
        private ISubject<int> _onPlusButtonClicked = new Subject<int>();
        private ISubject<int> _onMinusButtonClicked = new Subject<int>();
        private ISubject<float> _onSpeedChange = new Subject<float>();

        public IObservable<int> OnPlusButtonClicked => _onPlusButtonClicked;
        public IObservable<int> OnMinusButtonClicked => _onMinusButtonClicked;
        public IObservable<float> OnSpeedChange => _onSpeedChange;
        
        public void SetAmountOfBodies(int amountOfBodies)
        {
            _amountOfBodiesText.text = amountOfBodies.ToString();
        }

        private void OnEnable()
        {
            _disposables = new CompositeDisposable();
            _plus1Button.OnClickAsObservable().Subscribe(_ => PlusButtonHandler(1)).AddTo(_disposables);
            _plus10Button.OnClickAsObservable().Subscribe(_ => PlusButtonHandler(10)).AddTo(_disposables);
            _plus100Button.OnClickAsObservable().Subscribe(_ => PlusButtonHandler(100)).AddTo(_disposables);
            _minus1Button.OnClickAsObservable().Subscribe(_ => MinusButtonHandler(1)).AddTo(_disposables);
            _minus10Button.OnClickAsObservable().Subscribe(_ => MinusButtonHandler(10)).AddTo(_disposables);
            _minus100Button.OnClickAsObservable().Subscribe(_ => MinusButtonHandler(100)).AddTo(_disposables);
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

        private void PlusButtonHandler(int value)
        {
            _onPlusButtonClicked.OnNext(value);
        }

        private void MinusButtonHandler(int value)
        {
            _onMinusButtonClicked.OnNext(value);
        }
    }
}