using Data;
using UnityEngine;

namespace Game {

    public class CharacterWeaponLogic : AbstractCharacterLogic {

        [SerializeField]
        private Transform _weaponSpawnRoot;

        [SerializeField]
        private float _aimSpeed;

        private AbstractWeapon _weapon;

        private WeaponData _weaponData;

        public override void Init(Character character) {
            base.Init(character);
            _weapon.Init(character);
        }

        public void SetWeapon(WeaponData weaponData) {
            if(_weapon != null) {
                _weapon.gameObject.SetActive(false);
            }
            _weaponData = weaponData;
            _weapon = Instantiate(weaponData.WeaponPrefab, _weaponSpawnRoot);
        }

        public override void UpdateLogic() {
            base.UpdateLogic();
            if(_weapon == null) {
                return;
            }
            float aimAngle = Mathf.Atan2(Character.InputLogic.InputData.aimingVector.y, Character.InputLogic.InputData.aimingVector.x) * Mathf.Rad2Deg;
            //var direction = Vector3.RotateTowards(_abstractWeapon.transform.forward, Character.InputLogic.InputData.aimingVector, _aimSpeed * Time.deltaTime, 0.0f);
            var newRotationQuaternion = Quaternion.AngleAxis(-aimAngle - 90, Vector3.up);
            _weapon.transform.rotation = Quaternion.Lerp(_weapon.transform.rotation, newRotationQuaternion, _aimSpeed * Time.deltaTime);//Quaternion.LookRotation(Character.InputLogic.InputData.aimingVector);
            _weapon.UpdateWeapon();
            if(Character.InputLogic.InputData.shoot) {
                _weapon.Shoot();
            }
        }
    }
}