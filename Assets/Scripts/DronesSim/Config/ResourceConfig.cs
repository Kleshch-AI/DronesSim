using UnityEngine;

namespace DronesSim.Config
{
    [CreateAssetMenu(fileName = "ResourceConfig", menuName = "Configs/ResourceConfig")]
    public class ResourceConfig : ScriptableObject
    {
        public GameObject ResourceViewPrefab;
        public int SpawnInterval;
        public int MaxCount;
    }
}