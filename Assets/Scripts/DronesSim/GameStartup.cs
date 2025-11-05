using UnityEngine;

namespace DronesSim
{
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] ResourceSpawner resourceSpawner;
    
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = new GameManager(resourceSpawner);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            _gameManager.OnGamePause(pauseStatus);
        }
    }
}