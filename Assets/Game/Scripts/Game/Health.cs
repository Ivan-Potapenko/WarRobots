using NaughtyAttributes;
using System;
using UnityEngine;

namespace Game {

    public class Health : MonoBehaviour {

        [Serializable]
        public struct Damage {
            public int value;
        }

        public enum State {
            Death,
            Alive,
        }

        private State _currentState = State.Alive;
        public State CurrentState => _currentState;

        [SerializeField]
        private int _baseValue;
        public int BaseValue => _baseValue;

        private int _value;
        public int Value => _value;

        [SerializeField]
        private bool _hasRegeneration;

        [SerializeField]
        [ShowIf(nameof(_hasRegeneration))]
        private float _regenerationPercent;

        [SerializeField]
        [ShowIf(nameof(_hasRegeneration))]
        private float _timeBeforeStartRegenerateAfterDamage;

        [SerializeField]
        [ShowIf(nameof(_hasRegeneration))]
        private float _timeBetweenRegeneration;

        private float _currentTimeBeforeRegenerate;

        public event Action<Damage> onDamage;
        public event Action onDeath;
        public event Action onValueChanged;

        public void Init() {
            _value = _baseValue;
            _currentTimeBeforeRegenerate = _timeBetweenRegeneration;
        }

        public void DoDamage(Damage damage) {
            if (CurrentState == State.Death || damage.value <= 0) {
                return;
            }
            _value = Mathf.Max(0, _value - damage.value);
            onDamage?.Invoke(damage);
            onValueChanged?.Invoke();
            if (_value == 0) {
                _currentState = State.Death;
                onDeath?.Invoke();
            }
            _currentTimeBeforeRegenerate = _timeBeforeStartRegenerateAfterDamage;
        }

        public void UpdateHealth() {
            if (!_hasRegeneration || _value >= _baseValue || CurrentState == State.Death) {
                return;
            }
            if(_currentTimeBeforeRegenerate > 0) {
                _currentTimeBeforeRegenerate -= Time.deltaTime;
                return;
            }
            _value = (int)Mathf.Min(_value + _baseValue * _regenerationPercent, _baseValue);
            onValueChanged?.Invoke();
            _currentTimeBeforeRegenerate = _timeBetweenRegeneration;
        }
    }
}
