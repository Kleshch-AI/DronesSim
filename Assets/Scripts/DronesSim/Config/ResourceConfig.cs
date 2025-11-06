using UnityEngine;

namespace DronesSim.Config
{
    [CreateAssetMenu(fileName = "ResourceConfig", menuName = "Configs/ResourceConfig")]
    public class ResourceConfig : ScriptableObject
    {
        [SerializeField] private GameObject resourceViewPrefab;
        [SerializeField] private int spawnInterval;
        [SerializeField] private int maxCount;

        public GameObject ResourceViewPrefab => resourceViewPrefab;
        
        public int SpawnInterval =>spawnInterval;
        public int MaxCount =>maxCount;
    }
}