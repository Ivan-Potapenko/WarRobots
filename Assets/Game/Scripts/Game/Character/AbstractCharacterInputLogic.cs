using UnityEngine;

namespace Game {

    public abstract class AbstractCharacterInputLogic : AbstractCharacterLogic {

        public struct CharacterInputData {
            public Vector2 moveVector;
            public Vector2 aimingVector;
            public bool shoot;
        }

        protected CharacterInputData _inputData;
        public CharacterInputData InputData => _inputData;
    }
}