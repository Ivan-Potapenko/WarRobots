using System;
using UnityEngine;

namespace Game {

    [Serializable]
    public class AICondition {

        public enum Type {
            DistanceToDangerZone,
            DistanceToEnemy,
        }

        [SerializeField]
        private Type _type;

        [SerializeField]
        private AnimationCurve _curve;

        public float GetWeight(AIContext aIContext) {
            var valueToEvaluate = 0f;
            switch (_type) {
                case Type.DistanceToDangerZone:
                    valueToEvaluate = aIContext.distanceToDangerZone;
                    break;
                case Type.DistanceToEnemy:
                    if(aIContext.target == null) {
                        return 0;
                    }
                    valueToEvaluate = aIContext.distanceToTarget;
                    break;
            }
            return _curve.Evaluate(valueToEvaluate);
        }
    }
}