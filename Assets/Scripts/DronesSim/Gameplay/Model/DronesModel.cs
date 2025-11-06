using DronesSim.Config;
using UniRx;

namespace DronesSim.Gameplay.Model
{
    public class DronesModel
    {
        public ReactiveProperty<int> Speed { get; private set; }
        public ReactiveProperty<int> Amount { get; private set; }
        
        public int MaxAmount { get; private set; }
        
        public DronesModel(DronesConfig config)
        {
            Speed = new ReactiveProperty<int>(config.Speed);
            Amount = new ReactiveProperty<int>(config.Amount);
            MaxAmount = config.MaxAmount;
        }
    }
}