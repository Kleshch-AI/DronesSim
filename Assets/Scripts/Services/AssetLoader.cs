using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Services
{
    public static class AssetLoader
    {
        public static async Task<T> LoadAsync<T>(string path) where T : class
        {
            var handle = Addressables.LoadAssetAsync<T>(path);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"!!!!!!! Failed to load asset from {path}.");
                return null;
            }
        }
    }
}