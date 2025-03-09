using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Game {

    public class BotInputLogic : AbstractCharacterInputLogic {

        [SerializeField] private float detectionRadius = 10f; // Радиус обнаружения врага
        [SerializeField] private float moveRadius = 5f; // Радиус случайного движения к цели
        [SerializeField] private float strafeSpeed = 2f; // Скорость стрейфа
        [SerializeField] private float approachSpeed = 1f; // Скорость сближения
        [SerializeField] private float aimSpeed = 5f; // Скорость поворота прицела

        private Transform _target;
        private Vector3 _moveTarget;
        private Vector2 _aimDirection = Vector2.up;
        private bool _hasEnemy;
        private NavMeshPath _path;
        private int _currentCornerIndex;

        public override void Init(Character character) {
            base.Init(character);
            if(!Character.IsBot) {
                return;
            }
            _path = new NavMeshPath();
            SelectNewTarget();
        }

        public override void UpdateLogic() {
            base.UpdateLogic();
            if (!Character.IsBot) {
                return;
            }
            FindClosestEnemy();

            if (_hasEnemy) {
                EngageTarget();
            } else {
                MoveToTarget();
            }

            SmoothAim();
        }

        private void FindClosestEnemy() {
            var characters = BattleManager.instance.Characters
                .Where(c => c != Character)
                .OrderBy(c => Vector3.Distance(transform.position, c.transform.position))
                .ToList();

            if (characters.Count > 0) {
                var distance = Vector3.Distance(transform.position, characters[0].transform.position);
                if (distance <= detectionRadius) {
                    _target = characters[0].transform;
                    _hasEnemy = true;
                    return;
                }
            }

            _hasEnemy = false;
            _target = null;
        }

        private void EngageTarget() {
            if (_target == null) return;

            var toTarget = (transform.position - _target.position).normalized;
           // var strafeDirection = Vector3.Cross(toTarget, Vector3.up).normalized * (Random.value > 0.5f ? 1 : -1);
            var moveDirection = (toTarget * approachSpeed  /*strafeDirection * strafeSpeed*/).normalized;

            _inputData.moveVector = new Vector2(moveDirection.x, moveDirection.z);
            _inputData.shoot = true;
            _aimDirection = new Vector2(toTarget.x, toTarget.z);
        }

        private void MoveToTarget() {
            if (_path.corners.Length == 0 || _currentCornerIndex >= _path.corners.Length || Vector3.Distance(transform.position, _moveTarget) < 1f) {
                SelectNewTarget();
                return;
            }

            var nextCorner = _path.corners[_currentCornerIndex];
            var moveDirection = (nextCorner - transform.position).normalized;

            if (Vector3.Distance(transform.position, nextCorner) < 0.5f) {
                _currentCornerIndex++;
            }

            _inputData.moveVector = new Vector2(moveDirection.x, moveDirection.z);
            _inputData.shoot = false;
        }

        private void SelectNewTarget() {
            var characters = BattleManager.instance.Characters
                .Where(c => c != this)
                .OrderBy(c => Vector3.Distance(transform.position, c.transform.position))
                .ToList();

            if (characters.Count > 0) {
                var closestCharacterPos = characters[0].transform.position;
                var randomOffset = new Vector3(Random.Range(-moveRadius, moveRadius), 0, Random.Range(-moveRadius, moveRadius));
                _moveTarget = closestCharacterPos + randomOffset;

                if (NavMesh.CalculatePath(transform.position, _moveTarget, NavMesh.AllAreas, _path)) {
                    _currentCornerIndex = 0;
                }
            }
        }

        private void SmoothAim() {
            var currentAim = _inputData.aimingVector;
            _inputData.aimingVector = Vector2.Lerp(currentAim, _aimDirection, Time.deltaTime * aimSpeed);
        }
    }
}