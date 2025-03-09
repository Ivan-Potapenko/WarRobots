using UnityEngine;

namespace Game {

    public class Character : MonoBehaviour {

        [SerializeField]
        private AbstractCharacterInputLogic _botInputLogic;

        [SerializeField]
        private AbstractCharacterInputLogic _playerInputLogic;
        public AbstractCharacterInputLogic InputLogic => IsBot ? _botInputLogic : _playerInputLogic;

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

        [SerializeField]
        private bool _isBot;
        public bool IsBot => _isBot;

        private AbstractCharacterLogic[] _logic;

        public void Init(bool isBot) {
            _isBot = isBot;
            _logic = GetComponents<AbstractCharacterLogic>();
            foreach (var logic in _logic) {
                logic.Init(this);
            }
        }

        public void UpdateLogics() {
            if (!gameObject.activeSelf) {
                return;
            }
            foreach (var logic in _logic) {
                logic.UpdateLogic();
            }
        }
    }
}