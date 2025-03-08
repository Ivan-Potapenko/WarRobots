using UnityEngine;

namespace Game {

    public class PlayerInputLogic : AbstractCharacterInputLogic {

        public override void UpdateLogic() {
            base.UpdateLogic();
            _inputData.moveVector.x = Input.GetAxisRaw("Horizontal");
            _inputData.moveVector.y = Input.GetAxisRaw("Vertical");
            _inputData.aimingVector = Camera.main.WorldToScreenPoint(transform.position) - Input.mousePosition;
            Debug.Log($"AimingVector: {_inputData.aimingVector}");
        }
    }
}
