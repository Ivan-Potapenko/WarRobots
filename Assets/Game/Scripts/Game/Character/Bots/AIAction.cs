using NaughtyAttributes;
using UnityEngine;

namespace Game {

    [CreateAssetMenu(fileName = nameof(AIAction), menuName = "Data/Game/AIAction")]
    public class AIAction : ScriptableObject {

        public enum ActionType {
            MoveToEnemy,
            MoveToSafeZoneCenter,
        }

        [SerializeField]
        private AICondition[] _aIConditions;

        [SerializeField]
        private ActionType _actionType;

        [SerializeField]
        [ShowIf(nameof(_actionType), ActionType.MoveToSafeZoneCenter)]
        private float _distanceToMoveInZone;

        public float GetConditionWeight(AIContext aIContext) {
            var weightSum = 0f;
            for (var i = 0; i < _aIConditions.Length; i++) {
                weightSum += _aIConditions[i].GetWeight(aIContext);
            }
            return weightSum / _aIConditions.Length;
        }

        public void ModifyContextByEnemy(AIContext aIContext) {
            switch (_actionType) {
                case ActionType.MoveToEnemy:
                    MoveToEnemy(aIContext);
                    break;
                case ActionType.MoveToSafeZoneCenter:
                    MoveToSafeZone(aIContext);
                    break;
            }
        }

        private void MoveToEnemy(AIContext aIContext) {
            if (aIContext.target == null) {
                aIContext.targetPosition = aIContext.owner.transform.position;
                return;
            }
            aIContext.targetPosition = aIContext.target.transform.position;
        }

        private void MoveToSafeZone(AIContext aIContext) {
            aIContext.targetPosition = aIContext.owner.transform.position + (BattleManager.instance.GameLogic.Zone.transform.position - aIContext.owner.transform.position).normalized * _distanceToMoveInZone;
        }
    }
}