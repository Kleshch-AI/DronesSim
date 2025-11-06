using Sirenix.OdinInspector;
using UnityEngine;

namespace DronesSim.Config
{
    [CreateAssetMenu(fileName = "DronesConfig", menuName = "Configs/DronesConfig")]
    public class DronesConfig : SerializedScriptableObject
    {
        [SerializeField] private int speed;
        [SerializeField] private int amountPerBase;
        [SerializeField] private int maxAmount;

        public int Speed => speed;
        public int Amount => amountPerBase;
        public int MaxAmount => maxAmount;
    }
}