using UnityEngine;

namespace Game {

    public abstract class AbstractCharacterLogic : MonoBehaviour {

        private Character _character;
        public Character Character => _character;

        public virtual void Init(Character character) {
            _character = character;
        }

        public virtual void UpdateLogic() { }
    }
}
