using UnityEngine;

namespace Game {

    public class AnimatorCharacterLogic : AbstractCharacterLogic {

        [SerializeField]
        private string _speedAnimatorParam;

        public override void UpdateLogic() {
            base.UpdateLogic();
            Character.CharacterAnimator.SetFloat(_speedAnimatorParam, Character.InputLogic.InputData.moveVector.magnitude);
        }
    }
}

