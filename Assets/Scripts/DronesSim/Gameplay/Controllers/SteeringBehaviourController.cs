using DronesSim.Config;
using UnityEngine;

namespace DronesSim.Gameplay.Controllers
{
    public class SteeringBehaviourController : MonoBehaviour
    {
        [System.Flags]
        public enum Behaviours
        {
            None = 0,
            Seek = 1 << 0,
            Evade = 1 << 1,
        }

        private SteeringBehaviourSettings _behaviourSettings;

        private float _maxSpeed;
        private float _factor;
        private float _maxForce => _behaviourSettings.force * _factor;
        private float _avoidRadius => _behaviourSettings.evadeRadius * _factor;
        private float _avoidStrength => _behaviourSettings.evadeStrength * _factor;

        private Behaviours _activeBehaviours = Behaviours.None;
        private Vector2 _seekPosition;
        private LayerMask _separationMask;

        private Vector2 _velocity;

        public void Init(SteeringBehaviourSettings behaviourSettings)
        {
            _behaviourSettings = behaviourSettings;
        }

        public void SetSpeed(float speed)
        {
            _factor = speed / _behaviourSettings.speed;
            _maxSpeed = speed;
        }

        public void EnableBehaviour(Behaviours behaviour)
        {
            _activeBehaviours |= behaviour;
        }

        public void DisableBehaviour(Behaviours behaviour)
        {
            _activeBehaviours &= ~behaviour;
        }

        public void DisableAll()
        {
            _activeBehaviours = Behaviours.None;
        }

        public bool IsBehaviourEnabled(Behaviours behaviour)
        {
            return (_activeBehaviours & behaviour) != 0;
        }

        public void SetupSeek(Vector2 position) => _seekPosition = position;

        public void SetupEvade(LayerMask layerMask) => _separationMask = layerMask;

        private void Update()
        {
            if (_activeBehaviours == Behaviours.None)
                return;

            var steer = Vector2.zero;

            if (IsBehaviourEnabled(Behaviours.Seek))
            {
                steer += GetSeekForce();
#if UNITY_EDITOR
                Debug.DrawLine(transform.position, _seekPosition, Color.red);
#endif
            }

            if (IsBehaviourEnabled(Behaviours.Evade))
                steer += GetSeparationForce();

            // --- Clamp the force (limit how sharp we steer) ---
            steer = Vector2.ClampMagnitude(steer, _maxForce);

            // --- Update the velocity ---
            _velocity += steer * Time.deltaTime;
            _velocity = Vector2.ClampMagnitude(_velocity, _maxSpeed);

            // --- Move the drone ---
            transform.position += (Vector3)_velocity * Time.deltaTime;

            // --- (Optional) Face the movement direction ---
            if (_velocity.sqrMagnitude > 0.1f)
            {
                var angle = Mathf.Atan2(_velocity.y, _velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // Or use only forward vector if 3D
            }
        }

        private Vector2 GetSeekForce()
        {
            Vector2 desired = (_seekPosition - (Vector2)transform.position).normalized * _maxSpeed;
            return desired - _velocity;
        }

        private Vector2 GetSeparationForce()
        {
            var separation = Vector2.zero;
            var count = 0;

            var hits = Physics2D.OverlapCircleAll(transform.position, _avoidRadius, _separationMask);
            foreach (var hit in hits)
            {
                if (hit.gameObject == this.gameObject)
                    continue;

                var away = (Vector2)(transform.position - hit.transform.position);
                var dist = away.magnitude;
                if (dist > 0f)
                {
                    separation += away.normalized / dist; // stronger force the closer you are
                    count++;
                }
            }

            if (count > 0)
            {
                separation /= count;
                separation = separation.normalized * _avoidStrength;
            }

            return separation;
        }
    }
}