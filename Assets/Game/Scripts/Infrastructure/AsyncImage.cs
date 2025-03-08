using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Infrastructure {

    public class AsyncImage : MonoBehaviour {

        [SerializeField] 
        private AssetReference _loadableSprite;

        [SerializeField] 
        private Image _image;

        [SerializeField]
        private bool _loadOnAwake;

        private AsyncOperationHandle<Sprite>? _handle;

        private void Awake() {
            if (_loadOnAwake) {
                Load();
            }
        }

        public void Load() {
            LoadAsync(_loadableSprite);
        }

        public void Load(AssetReference assetReference) {
            LoadAsync(assetReference);
        }

        private async void LoadAsync(AssetReference assetReference) {
            if (_handle.HasValue && _handle.Value.IsValid()) {
                Addressables.Release(_handle.Value);
            }

            _handle = Addressables.LoadAssetAsync<Sprite>(assetReference);
            await _handle.Value.Task;

            if (_handle.Value.Status == AsyncOperationStatus.Succeeded) {
                _image.sprite = _handle.Value.Result;
            } else {
                Debug.LogError($"Ошибка загрузки спрайта {assetReference.RuntimeKey}");
            }
        }

        private void OnDestroy() {
            if (_handle.HasValue && _handle.Value.IsValid()) {
                Addressables.Release(_handle.Value);
            }
        }
    }
}

