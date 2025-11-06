using UniRx;
using UnityEngine;

namespace DronesSim.Gameplay.Controllers
{
    public class ResourceController : MonoBehaviour
    {
        private Subject<ResourceController> _onCollectResource = new Subject<ResourceController>();

        public bool IsFree { get; private set; } = true;

        public void Init(Vector3 position, Subject<ResourceController> onCollectResource)
        {
            _onCollectResource = onCollectResource;
            IsFree = true;
            transform.position = position;
            gameObject.SetActive(true);
            ResourceRegistry.RegisterResource(this);
        }

        public void Claim()
        {
            IsFree = false;
        }

        public void Free()
        {
            IsFree = true;
        }

        public void Collect()
        {
            IsFree = false;
            gameObject.SetActive(false);
            ResourceRegistry.UnregisterResource(this);
            _onCollectResource.OnNext(this);
        }

        private void OnDestroy()
        {
            ResourceRegistry.UnregisterResource(this);
        }
    }
}