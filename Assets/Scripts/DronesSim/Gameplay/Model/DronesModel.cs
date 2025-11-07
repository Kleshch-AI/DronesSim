using DronesSim.Config;
using UniRx;

namespace DronesSim.Gameplay.Model
{
    public class DronesModel
    {
        public ReactiveProperty<int> Amount { get; private set; }
        
        public DronesModel(DronesConfig config)
        {
            Amount = new ReactiveProperty<int>(config.Amount);
        }
    }
}