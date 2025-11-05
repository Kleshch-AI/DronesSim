using DronesSim.Config;
using Services;

namespace DronesSim
{
    public class GameManager
    {
        private ResourceSpawner _resourceSpawner;

        public GameManager(ResourceSpawner resourceSpawner)
        {
            _resourceSpawner = resourceSpawner;

            InitResources();
        }

        public void OnGamePause(bool isPaused)
        {
            //TODO
        }

        private async void InitResources()
        {
            var config = await AssetLoader.LoadAsync<ResourceConfig>("Configs/ResourceConfig");
            _resourceSpawner.Init(config);
        }
    }
}