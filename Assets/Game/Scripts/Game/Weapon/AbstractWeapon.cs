using UnityEngine;

namespace Game {

    public abstract class AbstractWeapon : MonoBehaviour {

        [SerializeField]
        private float _timeBetweenShoot;

        [SerializeField]
        private MuzzleFlash _muzzleFlash;

        [SerializeField]
        private Transform _shootPoint;
        public Transform ShootPoint => _shootPoint;

        private float _currenTimeBeforeShoot;

        private Character _owner;
        public Character Owner => _owner;

        public void Init(Character owner) {
            _owner = owner;
        }

        public void Shoot() {
            if (_currenTimeBeforeShoot > 0) {
                return;
            }
            _muzzleFlash.Play();
            ShootInternal();
            _currenTimeBeforeShoot = _timeBetweenShoot;
        }

        protected abstract void ShootInternal();

        public virtual void UpdateWeapon() {
            if (_currenTimeBeforeShoot > 0) {
                _currenTimeBeforeShoot -= Time.deltaTime;
            }
        }
    }
}