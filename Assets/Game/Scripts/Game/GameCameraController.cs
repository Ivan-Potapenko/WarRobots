using UnityEngine;

namespace Game {

    public class GameCameraController : MonoBehaviour {
        [SerializeField]
        private GameObject _player;

        [SerializeField]
        private float _smooth;

        private Vector3 _velocity = Vector3.zero;

        [SerializeField]
        private Vector3 _cameraPlayerPositionDifference;

        [SerializeField]
        private float _startYPosition;

        void Update() {
            UpdateCameraPosition();
        }

        private void UpdateCameraPosition() {
            if (_player == null) {
                return;
            }

            var newPosition = Vector3.SmoothDamp(transform.position, _player.gameObject.transform.position + _cameraPlayerPositionDifference, ref _velocity, _smooth);
            newPosition.y = _startYPosition;
            transform.position = newPosition;
        }
    }
}