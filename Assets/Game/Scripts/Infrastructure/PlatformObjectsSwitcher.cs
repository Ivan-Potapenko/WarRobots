using UnityEngine;
using YG;

namespace Infrastructure {

    public class PlatformObjectsSwitcher : MonoBehaviour {

        [SerializeField]
        private GameObject[] _desktopObjects;

        [SerializeField]
        private GameObject[] _mobileObjects;

        private void Awake() {
            if (YG2.envir.isDesktop) {
                SetObjectsActive(_desktopObjects,true);
                SetObjectsActive(_mobileObjects, false);
            } else {
                SetObjectsActive(_desktopObjects, false);
                SetObjectsActive(_mobileObjects, true);
            }
        }

        private void SetObjectsActive(GameObject[] gameObjects, bool enabled) {
            foreach (var gameObject in gameObjects) {
                gameObject.SetActive(enabled);
            }
        }
    }
}
