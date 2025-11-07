using System;

namespace DronesSim.Config
{
    [Serializable]
    public class SteeringBehaviourSettings
    {
        public float speed;
        public float force;
        public float evadeRadius;
        public float evadeStrength;
    }
}