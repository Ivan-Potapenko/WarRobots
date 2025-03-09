using UnityEngine;

namespace Game {

    public class CharacterVisualLogic : AbstractCharacterLogic {

        [SerializeField]
        private ParticleSystem _destroyEffectPrefab;

        private ParticleSystem _destroyEffectInstance;

        public override void Init(Character character) {
            base.Init(character);
            _destroyEffectInstance = Instantiate(_destroyEffectPrefab, Character.transform.parent);
            _destroyEffectInstance.gameObject.SetActive(false);
            Character.Health.onDeath += OnDeath;
        }

        private void OnDeath() {
            _destroyEffectInstance.gameObject.SetActive(true);
            _destroyEffectInstance.transform.position = Character.transform.position;
            _destroyEffectInstance.Play(withChildren: true);
        }
    }
}
