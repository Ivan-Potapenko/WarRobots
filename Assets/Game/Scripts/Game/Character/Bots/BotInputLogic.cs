using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Game {

    public class BotInputLogic : AbstractCharacterInputLogic {

        [SerializeField]
        private float _detectionRadius = 10f;

        [SerializeField]
        private float _aimSpeed = 5f;

        [SerializeField]
        private int _updateTargetTick;

        [SerializeField]
        private float _stopDistance = 1;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private AIAction[] _aIActions;

        [SerializeField]
        private float _timeToChangeMoveVector;

        private float _currentTimeToChangeMoveVector;

        private AIContext _aIContext = new AIContext();

        private int _currentTick;

        private Vector2 _aimDirection = Vector2.up;
        private NavMeshPath _path;
        private int _currentCornerIndex;

        private float _currentSpeed;

        private Vector3 _currentMoveTarget;

        private AIAction _currentAIAction;

        private bool _aiActionChanged;

        private bool TargetPositionReached => Vector3.Distance(transform.position, _currentMoveTarget) < _stopDistance || _currentCornerIndex >= _path.corners.Length || _path.corners.Length == 0;

        public override void Init(Character character) {
            base.Init(character);
            if (!Character.IsBot) {
                return;
            }
            _path = new NavMeshPath();
            ResetAIContext();
        }

        private void ResetAIContext() {
            _aIContext.owner = Character;
            _aIContext.distanceToTarget = float.MaxValue;
            _aIContext.distanceToDangerZone = BattleManager.instance.GameLogic.Zone.CalculateDistanceToDangerZone(Character.transform.position);
            _aIContext.target = null;
            _aIContext.targetPosition = Character.transform.position;
            _aiActionChanged = false;
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

            ResetAIContext();
            UpdateEnemyTarget();

            UpdateContextFromAction();

            if (TargetPositionReached || _currentTimeToChangeMoveVector < 0) {
                _currentMoveTarget = new Vector3(_aIContext.targetPosition.x + Random.Range(-_detectionRadius, _detectionRadius), 0, _aIContext.targetPosition.z + Random.Range(-_detectionRadius, _detectionRadius));
                if (NavMesh.CalculatePath(Character.transform.position, _currentMoveTarget, NavMesh.AllAreas, _path)) {
                    _currentCornerIndex = 0;
                    _currentSpeed = 0;
                }
                _currentTimeToChangeMoveVector = _timeToChangeMoveVector;
            }
            _currentTimeToChangeMoveVector -= Time.deltaTime;

            if (_aIContext.target != null) {
                var vectorToTarget = (transform.position - _aIContext.target.transform.position).normalized;
                _aimDirection = new Vector2(vectorToTarget.x, vectorToTarget.z);
            }
            else {
                _aimDirection = Character.transform.forward;
            }

            MoveToTarget();
            SmoothAim();
        }

        private void UpdateEnemyTarget() {
            var characters = BattleManager.instance.Characters
                .Where(c => c != Character && c.Health.CurrentState != Health.State.Death)
                .OrderBy(c => (transform.position - c.transform.position).sqrMagnitude)
                .ToList();
            if (characters.Count == 0) {
                _inputData.shoot = false;
                _aIContext.target = null;
                return;
            }
            var distance = Vector3.Distance(Character.transform.position, characters[0].transform.position);
            _aIContext.target = characters[0];
            _aIContext.distanceToTarget = distance;
            _inputData.shoot = distance <= Character.WeaponLogic.Weapon.AIShootDistance;
        }

        private void UpdateContextFromAction() {
            var actionToDo = _aIActions[0];
            var currentActionWeight = _aIActions[0].GetConditionWeight(_aIContext);
            for (var i = 1; i < _aIActions.Length; i++) {
                var weight = _aIActions[i].GetConditionWeight(_aIContext);
                if (weight > currentActionWeight) {
                    actionToDo = _aIActions[i];

                }
            }
            actionToDo.ModifyContextByEnemy(_aIContext);
            if(_currentAIAction != actionToDo) {
                _aiActionChanged = true;
                _currentAIAction = actionToDo;
            }
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