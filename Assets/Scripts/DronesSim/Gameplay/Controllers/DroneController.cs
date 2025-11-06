using System;
using UnityEngine;

namespace DronesSim.Gameplay.Controllers
{
    public enum DroneState
    {
        Inactive,
        SeekResource,
        MoveToResource,
        CollectResource,
        ReturnToBase,
        UnloadResource,
    }

    public class DroneController : MonoBehaviour
    {
        private float _seekInterval = 1f;

        private DroneState _state;
        private ResourceController _currentResource;
        private float _lastSeekTime = 0f;

        public void Init()
        {
        }

        public void Activate(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
            _state = DroneState.SeekResource;
        }

        public void Deactivate()
        {
            _state = DroneState.Inactive;
            _currentResource?.Free();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            switch (_state)
            {
                case DroneState.Inactive:
                {
                    return;
                }
                case DroneState.SeekResource:
                {
                    SeekResource();
                    break;
                }
                case DroneState.MoveToResource:
                {
                    if (_currentResource.IsFree)
                        MoveToResource();
                    else
                        _state = DroneState.SeekResource;
                    break;
                }
                case DroneState.CollectResource:
                {
                    CollectResource();
                    break;
                }
                case DroneState.ReturnToBase:
                {
                    ReturnToBase();
                    break;
                }
                case DroneState.UnloadResource:
                {
                    UnloadResource();
                    break;
                }
            }

            Debug.Log($"Drone [{gameObject.name}] --- State: {_state}]");
        }

        [Obsolete("Obsolete")]
        private void SeekResource()
        {
            if (Time.time - _lastSeekTime < _seekInterval)
                return;

            if (ResourceRegistry.GetNearestResource(transform.position, out _currentResource))
            {
                _state = DroneState.MoveToResource;
            }
        }

        private void MoveToResource()
        {
        }

        private void CollectResource()
        {
        }

        private void ReturnToBase()
        {
        }

        private void UnloadResource()
        {
        }
    }
}