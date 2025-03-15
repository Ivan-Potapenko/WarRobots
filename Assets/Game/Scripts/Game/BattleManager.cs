using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Game {

    public class BattleManager : MonoBehaviour {

        public static BattleManager instance;

        [SerializeField]
        private Transform[] _spawnPoints;

        [SerializeField]
        private GameCameraController _gameCamera;

        [SerializeField]
        private Character _characterPrefab;

        [SerializeField]
        private WeaponsConfig _weaponsConfig;

        private List<Health> _battleEntities = new List<Health>();
        public List<Health> BattleEntities => _battleEntities;

        private List<Character> _characters = new List<Character>();
        public List<Character> Characters => _characters;

        [SerializeField]
        private bool _disableBots;

        [SerializeField]
        private BattleRoyaleGameLogic _gameLogic;
        public BattleRoyaleGameLogic GameLogic => _gameLogic;

        public void Awake() {
            instance = this;
            GameLogic.Init();
            foreach (var spawnPoint in _spawnPoints) {
                var character = Instantiate(_characterPrefab, spawnPoint.position, spawnPoint.rotation, transform);
                character.gameObject.name = $"Bot_{_characters.Count.ToString()}";
                character.WeaponLogic.SetWeapon(_weaponsConfig.WeaponsData.PickRandom());
                _characters.Add(character);
                _battleEntities.Add(character.Health);
            }
            InitCharacters();
        }

        public void InitCharacters() {
            var localCharacter = _characters[Random.Range(0, _characters.Count)];
            localCharacter.Init(isBot: false);
            localCharacter.gameObject.name = "LocalCharacter";
            _gameCamera.Init(localCharacter);
            foreach (var character in _characters) {
                if (character != localCharacter) {
                    character.Init(isBot: true);
                }
            }
        }

        private void Update() {
            for (var i = 0; i < _characters.Count; i++) {
                if (_characters[i].IsBot && _disableBots) {
                    continue;
                }
                _characters[i].UpdateLogics();
            }
            _gameCamera.UpdateCamera();
            if (Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            _gameLogic.UpdateLogic();
        }
    }
}