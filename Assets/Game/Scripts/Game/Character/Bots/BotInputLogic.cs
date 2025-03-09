using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Game {

    public class BotInputLogic : AbstractCharacterInputLogic {

        private enum State {
            Follow,
            Attack,
        }

        [SerializeField]
        private float _detectionRadius = 10f; // Радиус обнаружения врага

        [SerializeField]
        private float _aimSpeed = 5f; // Скорость поворота прицела

        [SerializeField]
        private int _updateTargetTick;

        [SerializeField]
        private float _stopDistance = 1;

        [SerializeField]
        private float _speed;

        private int _currentTick;

        private Transform _target;
        private Vector3 _targetPosition;

        private Vector2 _aimDirection = Vector2.up;
        private NavMeshPath _path;
        private int _currentCornerIndex;

        private float _currentSpeed;

        private bool TargetPositionReached => Vector3.Distance(transform.position, _targetPosition) < _stopDistance || _currentCornerIndex >= _path.corners.Length || _path.corners.Length == 0;

        private State _currentState;

        public override void Init(Character character) {
            base.Init(character);
            if (!Character.IsBot) {
                return;
            }
            _targetPosition = character.transform.position;
            _path = new NavMeshPath();
        }

        public override void UpdateLogic() {
            base.UpdateLogic();
            if (!Character.IsBot) {
                return;
            }
            if (_currentTick > 0) {
                _currentTick--;
                return;
            }
            _currentTick = _updateTargetTick;

            FindClosestEnemy();

            if (_target == null) {
                return;
            }

            if (TargetPositionReached) {
                _targetPosition = new Vector3(_target.position.x + Random.Range(-_detectionRadius, _detectionRadius), 0, _target.position.z + Random.Range(-_detectionRadius, _detectionRadius));
                if (NavMesh.CalculatePath(Character.transform.position, _targetPosition, NavMesh.AllAreas, _path)) {
                    _currentCornerIndex = 0;
                    _currentSpeed = 0;
                }
            }
            var vectorToTarget = (transform.position - _target.position).normalized;
            _aimDirection = new Vector2(vectorToTarget.x, vectorToTarget.z);
            switch (_currentState) {
                case State.Attack:
                    _inputData.shoot = true;
                    break;
                case State.Follow:
                    _inputData.shoot = false;
                    break;
            }
            MoveToTarget();
            SmoothAim();
        }

        private void FindClosestEnemy() {
            var characters = BattleManager.instance.Characters
                .Where(c => c != Character && c.Health.CurrentState != Health.State.Death)
                .OrderBy(c => (transform.position - c.transform.position).sqrMagnitude)
                .ToList();
            if (characters.Count == 0) {
                _target = null;
                return;
            }
            for (var i = 0; i < characters.Count; i++) {
                var distance = Vector3.Distance(Character.transform.position, characters[0].transform.position);
                if (distance <= _detectionRadius) {
                    _target = characters[i].transform;
                    _currentState = State.Attack;
                    return;
                }
            }
            _target = characters[0].transform;
            _currentState = State.Follow;
        }

        private void FindFollowPosition() {

        }

        private void AttackTarget() {

        }

        private void MoveToTarget() {
            if (TargetPositionReached) {
                _inputData.moveVector = Vector3.zero;
                return;
            }
            _currentSpeed = Mathf.Lerp(_currentSpeed, 1, Time.deltaTime * _speed);
            var nextCorner = _path.corners[_currentCornerIndex];
            var moveDirection = (nextCorner - transform.position).normalized;
            if (Vector3.Distance(transform.position, nextCorner) < _stopDistance) {
                _currentCornerIndex++;
            }

            _inputData.moveVector = Vector2.Lerp(_inputData.moveVector, new Vector2(moveDirection.x, moveDirection.z), Time.deltaTime * _speed).normalized;
        }

        private void SmoothAim() {
            var currentAim = _inputData.aimingVector;
            _inputData.aimingVector = Vector2.Lerp(currentAim, _aimDirection, Time.deltaTime * _aimSpeed);
        }
    }
}