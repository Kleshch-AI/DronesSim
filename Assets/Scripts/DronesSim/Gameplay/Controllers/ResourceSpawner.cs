using System;
using System.Collections.Generic;
using DronesSim.Gameplay.Model;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DronesSim.Gameplay.Controllers
{
    public class ResourceSpawner : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D spawnArea;

        private ResourceModel _model;
        private GameObject _resourceViewPrefab;

        private int _count;

        private Queue<GameObject> _resourcePool = new Queue<GameObject>();

        public void Init(ResourceModel model, GameObject resourceViewPrefab)
        {
            _model = model;
            _resourceViewPrefab = resourceViewPrefab;

            Observable
                .Interval(TimeSpan.FromSeconds(_model.SpawnInterval.Value))
                .Where(_ => _count < _model.MaxCount.Value)
                .Subscribe(_ => SpawnResource()).AddTo(this);
        }

        private void SpawnResource()
        {
            var pos = GetRandomPosition();
            var resource = _resourcePool.Count == 0
                ? Instantiate(_resourceViewPrefab, pos, Quaternion.identity)
                : _resourcePool.Dequeue();
            resource.transform.position = pos;
            resource.SetActive(true);
            _count++;
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

        private void DestroyResource(GameObject resource)
        {
            resource.SetActive(false);
            _resourcePool.Enqueue(resource);
            _count--;
        }
    }
}