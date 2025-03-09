using System.Collections.Generic;
using UnityEngine;

namespace Game {

    public class BattleManager : MonoBehaviour {

        public static BattleManager instance;

        [SerializeField]
        private Transform[] _spawnPoints;

        [SerializeField]
        private GameCameraController _gameCamera;

        [SerializeField]
        private Character _characterPrefab;

        private List<Health> _battleEntities = new List<Health>();
        public List<Health> BattleEntities => _battleEntities;

        private List<Character> _characters = new List<Character>();
        public List<Character> Characters => _characters;

        public void Awake() {
            instance = this;
            foreach (var spawnPoint in _spawnPoints) {
                var character = Instantiate(_characterPrefab, spawnPoint.position, spawnPoint.rotation, transform);
                character.gameObject.name = _characters.Count.ToString();
                _characters.Add(character);
                _battleEntities.Add(character.Health);
            }
            InitCharacters();
        }

        public void InitCharacters() {
            var localCharacter = _characters[Random.Range(0, _characters.Count)];
            localCharacter.Init(isBot: false);
            _gameCamera.Init(localCharacter);
            foreach (var character in _characters) {
                if (character != localCharacter) {
                    character.Init(isBot: true);
                }
            }
        }

        private void Update() {
            for (var i = 0; i < _characters.Count; i++) {
                _characters[i].UpdateLogics();
            }
            _gameCamera.UpdateCamera();
        }
    }
}