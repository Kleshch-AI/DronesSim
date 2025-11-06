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
        [SerializeField] private ResourceController resourceViewPrefab;

        private ResourceModel _model;

        private int _count;

        private Queue<ResourceController> _resourcePool = new Queue<ResourceController>();

        private Subject<ResourceController> _onCollectResource = new Subject<ResourceController>();

        public void Init(ResourceModel model)
        {
            _model = model;

            Observable
                .Interval(TimeSpan.FromSeconds(_model.SpawnInterval.Value))
                .Where(_ => _count < _model.MaxCount.Value)
                .Subscribe(_ => SpawnResource()).AddTo(this);

            _onCollectResource.Subscribe(OnCollectResource).AddTo(this);
        }

        private void SpawnResource()
        {
            var pos = GetRandomPosition();
            var resource = _resourcePool.Count == 0
                ? Instantiate(resourceViewPrefab, pos, Quaternion.identity)
                : _resourcePool.Dequeue();
            resource.Init(pos, _onCollectResource);
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

        private void OnCollectResource(ResourceController resource)
        {
            _resourcePool.Enqueue(resource);
            _count--;
        }
    }
}