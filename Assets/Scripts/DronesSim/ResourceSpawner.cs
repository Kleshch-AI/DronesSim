using System.Collections.Generic;
using DronesSim.Config;
using UnityEngine;

namespace DronesSim
{
    public class ResourceSpawner : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D spawnArea;

        private ResourceConfig  _config;

        private bool _isInitialized = false;

        private float _timer = 0f;
        private int _count;

        private Queue<GameObject> _resourcePool = new Queue<GameObject>();

        public void Init(ResourceConfig config)
        {
            _config = config;
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized)
                return;

            if (_count >= _config.MaxCount)
                return;

            _timer += Time.deltaTime;

            if (_timer < _config.SpawnInterval) 
                return;
            
            SpawnResource();
            _timer = 0f;
        }

        private void SpawnResource()
        {
            var pos = GetRandomPosition();
            var resource = _resourcePool.Count == 0
                ? Instantiate(_config.ResourceViewPrefab, pos, Quaternion.identity)
                : _resourcePool.Dequeue();
            resource.transform.position = pos;
            resource.SetActive(true);
            _count++;
        }

        private void DestroyResource(GameObject resource)
        {
            resource.SetActive(false);
            _resourcePool.Enqueue(resource);
            _count--;
        }

        private Vector3 GetRandomPosition()
        {
            var bounds = spawnArea.bounds;
            return new Vector3
            (
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                0
            );
        }
    }
}