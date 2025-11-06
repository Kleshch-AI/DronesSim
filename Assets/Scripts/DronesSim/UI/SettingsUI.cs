using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DronesSim.UI
{
    public class SettingsUI : MonoBehaviour
    {
        public struct Ctx
        {
            public ReactiveProperty<int> DronesAmount;

            public int DronesMaxAmount;
        }

        [SerializeField] private Slider dronesAmountSlider;
        [SerializeField] private TMP_Text dronesAmountText;

        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            dronesAmountSlider.maxValue = _ctx.DronesMaxAmount;
            dronesAmountSlider.value = _ctx.DronesAmount.Value;

            dronesAmountSlider
                .OnValueChangedAsObservable()
                .Subscribe(value => dronesAmountText.text = ((int)value).ToString())
                .AddTo(this);

            dronesAmountSlider
                .OnValueChangedAsObservable()
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Subscribe(value => _ctx.DronesAmount.Value = (int)value)
                .AddTo(this);
        }
    }
}