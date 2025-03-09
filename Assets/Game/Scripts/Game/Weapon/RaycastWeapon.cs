using System.Collections.Generic;
using UnityEngine;

namespace Game {

    public class RaycastWeapon : AbstractWeapon {

        [SerializeField]
        private ParticleSystem _bulletEffect;

        [SerializeField]
        private Collider _shootCollider;

        [SerializeField]
        private LayerMask _raycastLayerMask;

        [SerializeField]
        private float _raycastDistance;

        [SerializeField]
        private Health.Damage _damage;

        private List<Character> _entitiesToShoot = new List<Character>();

        protected override void ShootInternal() {
            _entitiesToShoot.Clear();
            _bulletEffect.transform.rotation = ShootPoint.rotation;
            foreach (var entity in BattleManager.instance.Characters) {
                if (IsColliding(_shootCollider, entity.CharacterController)) {
                    _entitiesToShoot.Add(entity);
                }
            }
            foreach (var entity in _entitiesToShoot) {
                if (Physics.Raycast(ShootPoint.transform.position, entity.CharacterController.bounds.center - ShootPoint.transform.position, out var hitInfo, _raycastDistance, _raycastLayerMask)) {
                    if (hitInfo.collider.gameObject == entity.gameObject) {
                        _bulletEffect.transform.LookAt(entity.CharacterController.bounds.center);
                        entity.Health.DoDamage(_damage);
                    }
                }
            }
            _bulletEffect.Play();
        }

        private bool IsColliding(Collider col1, Collider col2) {
            return Physics.ComputePenetration(
                col1, col1.transform.position, col1.transform.rotation,
                col2, col2.transform.position, col2.transform.rotation,
                out _, out _
            );
        }
    }
}