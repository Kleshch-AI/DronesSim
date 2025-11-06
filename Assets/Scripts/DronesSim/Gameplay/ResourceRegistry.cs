using System.Collections.Generic;
using DronesSim.Gameplay.Controllers;
using UnityEngine;

namespace DronesSim.Gameplay
{
    public static class ResourceRegistry
    {
        private static List<ResourceController> _allResources = new List<ResourceController>();

        public static IReadOnlyList<ResourceController> GetAllResources()
        {
            return _allResources;
        }

        public static IReadOnlyList<ResourceController> GetFreeResources()
        {
            return _allResources.FindAll(x => x.IsFree);
        }

        public static void RegisterResource(ResourceController resource)
        {
            if (!_allResources.Contains(resource))
                _allResources.Add(resource);
        }

        public static void UnregisterResource(ResourceController resource)
        {
            if (_allResources.Contains(resource))
                _allResources.Remove(resource);
        }

        public static bool GetNearestResource(Vector2 position, out  ResourceController nearestResource)
        {
            nearestResource = null;
            var minSqr = float.MaxValue;
            foreach (var resource in _allResources)
            {
                if (!resource.IsFree)
                    continue;

                var sqr = ((Vector2)resource.transform.position - position).sqrMagnitude;
                if (sqr > minSqr)
                    continue;

                nearestResource = resource;
                minSqr = sqr;
            }

            return minSqr < float.MaxValue;
        }
    }
}