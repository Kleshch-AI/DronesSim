using DronesSim.Config;
using DronesSim.Gameplay.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DronesSim.UI
{
    public class UIManager : MonoBehaviour
    {
        public struct Ctx
        {
            public DronesModel DronesModel;
            public DronesConfig DronesConfig;
        }
        
        [SerializeField] private Button settingsButton;
        [SerializeField] private SettingsUI settingsUI;

        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
            
            settingsUI.Init(new SettingsUI.Ctx
            {
                DronesAmount = _ctx.DronesModel.Amount,
                DronesMaxAmount = _ctx.DronesConfig.MaxAmount,
            });
            
            settingsButton.OnClickAsObservable().Subscribe(_ => ToggleSettings()).AddTo(this);
        }

        private void ToggleSettings()
        {
            settingsUI.gameObject.SetActive(!settingsUI.gameObject.activeInHierarchy);
        }
    }
}