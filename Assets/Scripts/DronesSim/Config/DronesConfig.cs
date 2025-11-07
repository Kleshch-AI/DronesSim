using System;
using UnityEngine;

namespace DronesSim.Config
{
    [CreateAssetMenu(fileName = "DronesConfig", menuName = "Configs/DronesConfig")]
    public class DronesConfig : ScriptableObject
    {
        [Serializable]
        public class DronesMovementSettings
        {
            public float collectDuration;
            public float unloadDuration;
            public float collectDistance;
            public float baseContactDistance;
            public float claimResourceDistance;
        }
        
        [SerializeField] private float globalSteeringSpeed;
        [SerializeField] private int amountPerBase;
        [SerializeField] private int maxAmount;
        [SerializeField] private DronesMovementSettings movementSettings;
        [SerializeField] private SteeringBehaviourSettings steeringBehaviourSettings;
        
        public float GlobalSteeringSpeed => globalSteeringSpeed;
        public int Amount => amountPerBase;
        public int MaxAmount => maxAmount;
        public DronesMovementSettings MovementSettings => movementSettings;
        public SteeringBehaviourSettings SteeringBehaviourSettings => steeringBehaviourSettings;
    }
}