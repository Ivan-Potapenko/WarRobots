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
        private int _value;
        public int Value => _value;

        public event Action<Damage> onDamage;
        public event Action onDeath;

        public void DoDamage(Damage damage) {
            if(damage.value < 0) {
                return;
            }
            _value = Mathf.Max(0, _value - damage.value);
            onDamage?.Invoke(damage);
            if(_value == 0) {
                _currentState = State.Death;
                onDeath?.Invoke();
            }
        }
    }
}
