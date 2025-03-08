using UnityEngine;

namespace Game {

    public class Character : MonoBehaviour {

        [SerializeField]
        private AbstractCharacterInputLogic _inputLogic;
        public AbstractCharacterInputLogic InputLogic => _inputLogic;

        [SerializeField]
        private CharacterMoveLogic _moveLogic;
        public CharacterMoveLogic MoveLogic => _moveLogic;

        [SerializeField]
        private CharacterController _characterController;
        public CharacterController CharacterController => _characterController;

        [SerializeField]
        private Health _health;
        public Health Health => _health;

        [SerializeField]
        private Animator _characterAnimator;
        public Animator CharacterAnimator => _characterAnimator;

        private AbstractCharacterLogic[] _logic;

        public void Start() {
            _logic = GetComponents<AbstractCharacterLogic>();
            foreach (var logic in _logic) {
                logic.Init(this);
            }
        }

        public void Update() {
            if (!gameObject.activeSelf) {
                return;
            }
            foreach (var logic in _logic) {
                logic.UpdateLogic();
            }
        }
    }
}