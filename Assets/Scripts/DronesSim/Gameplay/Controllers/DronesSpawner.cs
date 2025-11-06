using System.Collections.Generic;
using DronesSim.Gameplay.Model;
using UniRx;
using UnityEngine;

namespace DronesSim.Gameplay.Controllers
{
    public class DronesSpawner : MonoBehaviour
    {
        [SerializeField] private DroneType _type;

        [SerializeField] private DroneController _dronePrefab;

        private DronesModel _model;

        private Queue<DroneController> _drones = new Queue<DroneController>();
        private Queue<DroneController> _dronesPool = new Queue<DroneController>();

        private int _activeDronesCount = 0;

        public void Init(DronesModel model)
        {
            _model = model;

            _model.Amount.Subscribe(OnDronesAmountChange).AddTo(this);
        }

        private void OnDronesAmountChange(int amount)
        {
            if (_activeDronesCount > amount)
            {
                for (var i = 0; i < _activeDronesCount - amount; i++)
                    DeactivateDrone();
            }
            else if (_activeDronesCount < amount)
            {
                for (var i = 0; i < amount - _activeDronesCount; i++)
                    CreateDrone();
            }
            _activeDronesCount = amount;
        }

        private void CreateDrone()
        {
            var drone = _dronesPool.Count > 0
                ? _dronesPool.Dequeue()
                : Instantiate(_dronePrefab, transform.position, transform.rotation);
            _drones.Enqueue(drone);
            drone.gameObject.SetActive(true);
        }

        private void DeactivateDrone()
        {
            var droneToDeactivate = _drones.Dequeue();
            droneToDeactivate.gameObject.SetActive(false);
            _dronesPool.Enqueue(droneToDeactivate);
        }
    }
}