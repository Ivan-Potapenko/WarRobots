using UnityEngine;

namespace Game {

    public class CharacterWeaponLogic : AbstractCharacterLogic {

        [SerializeField]
        private AbstractWeapon _abstractWeapon;

        [SerializeField]
        private Transform _weaponSpawnRoot;

        [SerializeField]
        private float _aimSpeed;

        public override void UpdateLogic() {
            base.UpdateLogic();
            float aimAngle = Mathf.Atan2(Character.InputLogic.InputData.aimingVector.y, Character.InputLogic.InputData.aimingVector.x) * Mathf.Rad2Deg;
            //var direction = Vector3.RotateTowards(_abstractWeapon.transform.forward, Character.InputLogic.InputData.aimingVector, _aimSpeed * Time.deltaTime, 0.0f);
            var newRotationQuaternion = Quaternion.AngleAxis(-aimAngle - 90, Vector3.up);
            _abstractWeapon.transform.rotation = Quaternion.Lerp(_abstractWeapon.transform.rotation, newRotationQuaternion, _aimSpeed * Time.deltaTime);//Quaternion.LookRotation(Character.InputLogic.InputData.aimingVector);
            _abstractWeapon.UpdateWeapon();
            if(Character.InputLogic.InputData.shoot) {
                _abstractWeapon.Shoot();
            }
        }
    }
}