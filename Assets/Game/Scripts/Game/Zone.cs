using System;
using System.Collections;
using UnityEngine;

namespace Game {

    public class Zone : MonoBehaviour {

        [Serializable]
        private struct Stage {
            public Vector3 center;
            public Vector3 size;
        }

        [SerializeField]
        private Stage[] _stages;

        [SerializeField]
        private float _timeToSwitchStage;

        [SerializeField]
        private Transform _borderTransform;

        private int _currentStage;

        private float _radius;
        public float Radius => _radius;

        public void Init() {
            _currentStage = 0;
            UpdateZoneProgress(1);
            StartCoroutine(SwitchStage());
        }

        private IEnumerator SwitchStage() {
            var currentTime = _timeToSwitchStage;
            while (currentTime > 0) {
                yield return null;
                currentTime -= Time.deltaTime;
                var progress = currentTime / _timeToSwitchStage;
                UpdateZoneProgress(progress);
            }
            UpdateZoneProgress(0);
            _currentStage++;
        }

        private void UpdateZoneProgress(float progress) {
            transform.localScale = Vector3.Lerp(_stages[_currentStage + 1].size, _stages[_currentStage].size, progress);
            transform.position = Vector3.Lerp(_stages[_currentStage + 1].center, _stages[_currentStage].center, progress);
            _radius = (_borderTransform.position - transform.position).magnitude;
        }

        public bool InSafeZone(Vector3 position) {
            position.y = transform.position.y;
            return (position - transform.position).sqrMagnitude <= _radius * _radius;
        }

        public float CalculateDistanceToDangerZone(Vector3 position) {
            position.y = transform.position.y;
            return _radius - (position - transform.position).magnitude;
        }
    }
}