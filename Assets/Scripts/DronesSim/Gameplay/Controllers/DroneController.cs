using DronesSim.Config;
using UnityEngine;
using Behaviours = DronesSim.Gameplay.Controllers.SteeringBehaviourController.Behaviours;

namespace DronesSim.Gameplay.Controllers
{
    public enum DroneState
    {
        Inactive,
        FindResource,
        MoveToResource,
        CollectResource,
        ReturnToBase,
        UnloadResource,
    }

    [RequireComponent(typeof(SteeringBehaviourController))]
    public class DroneController : MonoBehaviour
    {
        [SerializeField] private SteeringBehaviourController steering;

        private DronesConfig.DronesMovementSettings _movementSettings;
        private float _seekInterval = 1f;
        private Vector3 _basePosition;

        private DroneState _state;

        private ResourceController _currentResource;
        private float _lastSeekTime = 0f;
        private float _timer = 0f;
        private bool _currentResourceClaimed = false;

        public void Init(DronesConfig config, Vector3 basePosition, float speed)
        {
            _movementSettings = config.MovementSettings;
            _basePosition = basePosition;
            
            steering.Init(config.SteeringBehaviourSettings);
            steering.SetSpeed(speed);
            steering.SetupEvade(LayerMask.GetMask("Drones"));
        }

        public void Activate(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
            UpdateState(DroneState.FindResource);
        }

        public void Deactivate()
        {
            UpdateState(DroneState.Inactive);
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
                case DroneState.FindResource:
                {
                    _timer += Time.deltaTime;
                    if (_timer < _seekInterval)
                        return;

                    _timer = 0f;
                    if (ResourceRegistry.GetNearestResource(transform.position, out _currentResource))
                    {
                        _currentResourceClaimed = false;
                        steering.SetupSeek(_currentResource.transform.position);
                        UpdateState(DroneState.MoveToResource);
                    }

                    break;
                }
                case DroneState.MoveToResource:
                {
                    if (!_currentResourceClaimed && !_currentResource.IsFree)
                        UpdateState(DroneState.FindResource);

                    var distance = (transform.position - _currentResource.transform.position).sqrMagnitude;
                    if (!_currentResourceClaimed && distance <= _movementSettings.claimResourceDistance)
                    {
                        _currentResource.Claim();
                        _currentResourceClaimed = true;
                    }

                    if (distance > _movementSettings.collectDistance)
                        return;

                    UpdateState(DroneState.CollectResource);
                    break;
                }
                case DroneState.CollectResource:
                {
                    _timer += Time.deltaTime;
                    if (_timer < _movementSettings.collectDuration)
                        return;

                    _timer = 0f;
                    steering.SetupSeek(_basePosition);
                    UpdateState(DroneState.ReturnToBase);
                    _currentResource.Collect();
                    _currentResource = null;
                    break;
                }
                case DroneState.ReturnToBase:
                {
                    var distance = (transform.position - _basePosition).sqrMagnitude;
                    if (distance > _movementSettings.baseContactDistance)
                        return;

                    UpdateState(DroneState.UnloadResource);
                    break;
                }
                case DroneState.UnloadResource:
                {
                    _timer += Time.deltaTime;
                    if (_timer < _movementSettings.unloadDuration)
                        return;

                    _timer = 0f;
                    UpdateState(DroneState.FindResource);
                    break;
                }
            }
        }

        private void UpdateState(DroneState state)
        {
            _state = state;
            UpdateSteering();
        }

        private void UpdateSteering()
        {
            switch (_state)
            {
                case DroneState.Inactive:
                    steering.DisableAll();
                    break;

                case DroneState.FindResource:
                    steering.DisableBehaviour(Behaviours.Seek);
                    steering.EnableBehaviour(Behaviours.Evade);
                    // steering.DisableAll();
                    break;

                case DroneState.MoveToResource:
                    steering.EnableBehaviour(Behaviours.Seek);
                    steering.EnableBehaviour(Behaviours.Evade);
                    break;

                case DroneState.CollectResource:
                    steering.DisableAll();
                    break;

                case DroneState.ReturnToBase:
                    steering.EnableBehaviour(Behaviours.Seek);
                    steering.EnableBehaviour(Behaviours.Evade);
                    break;

                case DroneState.UnloadResource:
                    steering.DisableAll();
                    break;
            }
        }
    }
}