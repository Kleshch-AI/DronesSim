using System.Collections.Generic;
using DronesSim.Gameplay.Controllers;
using DronesSim.UI;
using UnityEngine;

namespace DronesSim
{
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private ResourceSpawner resourceSpawner;
        [SerializeField] private List<DronesSpawner> dronesSpawners;
        [SerializeField] private UIManager uiManager;
    
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = new GameManager();
            _gameManager.InitGame(resourceSpawner, dronesSpawners, uiManager);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            _gameManager.OnGamePause(pauseStatus);
        }
    }
}