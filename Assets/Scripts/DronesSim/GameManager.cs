using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DronesSim.Config;
using DronesSim.Gameplay.Controllers;
using DronesSim.Gameplay.Model;
using DronesSim.UI;
using Services;

namespace DronesSim
{
    public class GameManager
    {
        private struct Models
        {
            public ResourceModel ResourceModel;
            public DronesModel DronesModel;
        }

        private struct Configs
        {
            public DronesConfig DronesConfig;
            public ResourceConfig ResourceConfig;
        }

        private Models _models;
        private Configs _configs;

        public async void InitGame(ResourceSpawner resourceSpawner, List<DronesSpawner> dronesSpawners,
            UIManager uiManager)
        {
            try
            {
                var loadOperations = new List<Task>
                {
                    InitResources(resourceSpawner),
                    InitBases(dronesSpawners)
                };

                await Task.WhenAll(loadOperations);

                uiManager.Init(new UIManager.Ctx
                {
                    DronesModel = _models.DronesModel,
                    DronesConfig = _configs.DronesConfig,
                });
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }

        public void OnGamePause(bool isPaused)
        {
            //TODO
        }

        private async Task InitResources(ResourceSpawner resourceSpawner)
        {
            _configs.ResourceConfig = await AssetLoader.LoadAsync<ResourceConfig>("Configs/ResourceConfig");
            _models.ResourceModel = new ResourceModel(_configs.ResourceConfig);
            resourceSpawner.Init(_configs.ResourceConfig);
        }

        private async Task InitBases(List<DronesSpawner> droneSpawners)
        {
            _configs.DronesConfig = await AssetLoader.LoadAsync<DronesConfig>("Configs/DronesConfig");
            _models.DronesModel = new DronesModel(_configs.DronesConfig);
            foreach (var b in droneSpawners)
                b.Init(_models.DronesModel, _configs.DronesConfig);
        }
    }
}