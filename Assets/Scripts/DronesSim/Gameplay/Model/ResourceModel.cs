using DronesSim.Config;
using UniRx;

namespace DronesSim.Gameplay.Model
{
    public class ResourceModel
    {
        public ReactiveProperty<int> SpawnInterval { get; private set; }
        public ReactiveProperty<int> MaxCount { get; private set; }
        
        public ResourceModel(ResourceConfig config)
        {
            SpawnInterval =  new ReactiveProperty<int>(config.SpawnInterval);
            MaxCount = new ReactiveProperty<int>(config.MaxCount);
        }
    }
}