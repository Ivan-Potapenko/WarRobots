using UnityEngine;

namespace Game {

    public class GameCameraController : MonoBehaviour {

        [SerializeField]
        private float _smooth;

        private Vector3 _velocity = Vector3.zero;

        [SerializeField]
        private Vector3 _cameraPlayerPositionDifference;

        [SerializeField]
        private float _startYPosition;

        private Character _character;

        public void Init(Character character) {
            _character = character;
            transform.position = _character.gameObject.transform.position + _cameraPlayerPositionDifference;
        }

        public void UpdateCamera() {
            UpdateCameraPosition();
        }

        private void UpdateCameraPosition() {
            if (_character == null) {
                return;
            }

            var newPosition = Vector3.SmoothDamp(transform.position, _character.gameObject.transform.position + _cameraPlayerPositionDifference + Vector3.up * GetWeaponHightModifier(), ref _velocity, _smooth);
            //newPosition.y = _startYPosition;
            transform.position = newPosition;
        }

        private float GetWeaponHightModifier() {
            var weapon = _character.WeaponLogic.Weapon;
            if (weapon == null) {
                return 0;
            }
            return _cameraPlayerPositionDifference.y * weapon.CameraHightModifier - _cameraPlayerPositionDifference.y;
        }
    }
}