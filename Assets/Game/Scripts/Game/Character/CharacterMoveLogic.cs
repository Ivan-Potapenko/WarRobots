using System;
using UnityEngine;

namespace Game {

    public class CharacterMoveLogic : AbstractCharacterLogic {

        [SerializeField]
        private float _moveSpeed;
        private float CurrentMoveSpeed => _moveSpeed;

        [SerializeField]
        private float _rotationSpeed;

        [SerializeField]
        private float _gravityForce;

        public event Action onStartMove;
        public event Action onStopMove;

        private bool _isMoving;
        public bool IsMoving => _isMoving;

        private float _currentGravityForce;

        public override void UpdateLogic() {
            base.UpdateLogic();
            UpdateCharacterMove();
        }

        private void UpdateCharacterMove() {
            // UpdateGravity();
            var normalizedInputVector = Character.InputLogic.InputData.moveVector.normalized;
            var moveVector = new Vector3(normalizedInputVector.x, 0, normalizedInputVector.y);
            UpdateRotation(moveVector);
            moveVector = moveVector * CurrentMoveSpeed * Time.deltaTime;

            if (moveVector.magnitude != 0 && !_isMoving) {
                onStartMove?.Invoke();
                _isMoving = true;

            } else if (moveVector.magnitude == 0 && _isMoving) {
                _isMoving = false;
                onStopMove?.Invoke();
            }
            //moveVector.y = _gravityForce;
            Character.CharacterController.Move(moveVector);
        }

        private void UpdateRotation(Vector3 moveVector) {
            var direction = Vector3.RotateTowards(transform.forward, moveVector, _rotationSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private void UpdateGravity() {
            if (!Character.CharacterController.isGrounded) {
                _currentGravityForce -= _gravityForce * Time.deltaTime;
            } else {
                _currentGravityForce = -1f;
            }
        }
    }
}