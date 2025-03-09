using UnityEngine;

namespace Game {

    public class CharacterHealthVisualizer : AbstractCharacterLogic {

        [SerializeField]
        private Vector3 _offsetPosition;

        [SerializeField]
        private HeathVisualizer _heathVisualizerPrefab;

        private HeathVisualizer _heathVisualizerInstance;

        public override void Init(Character character) {
            base.Init(character);
            _heathVisualizerInstance = Instantiate(_heathVisualizerPrefab, Character.transform.position + _offsetPosition, Quaternion.identity, Character.transform.parent);
            _heathVisualizerInstance.Init(Character.Health);
        }

        public override void UpdateLogic() {
            base.UpdateLogic();
            _heathVisualizerInstance.transform.position = _offsetPosition + Character.transform.position;
        }
    }
}