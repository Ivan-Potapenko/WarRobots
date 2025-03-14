using System.Collections.Generic;
using UnityEngine;

namespace Game {

    public class RaycastWeapon : AbstractWeapon {

        [SerializeField]
        private ParticleSystem _bulletEffect;

        [SerializeField]
        private WeaponCone _weaponCone;

        [SerializeField]
        private LayerMask _raycastLayerMask;

        [SerializeField]
        private float _raycastDistance;

        [SerializeField]
        private float _damage;

        [SerializeField]
        private AnimationCurve _damageCurve;

        //[SerializeField]
        //private float _angleToDamage;

        private List<Character> _entitiesToShoot = new List<Character>();

        protected override void ShootInternal() {
            _entitiesToShoot.Clear();
            _bulletEffect.transform.rotation = ShootPoint.rotation;
            foreach (var entity in BattleManager.instance.Characters) {
                if(entity == Owner) {
                    continue;
                }
                if (_weaponCone.IsPointInTrapezoid(entity.transform.position)) {
                    _entitiesToShoot.Add(entity);
                }
            }
            foreach (var entity in _entitiesToShoot) {
                var vectorToEnemy = entity.CharacterController.bounds.center - ShootPoint.transform.position;
                var vectorToCheckAngle = vectorToEnemy;
                vectorToCheckAngle.y = 0;
                if (Physics.Raycast(ShootPoint.transform.position, entity.CharacterController.bounds.center - ShootPoint.transform.position, out var hitInfo, _raycastDistance, _raycastLayerMask)) {
                    if (hitInfo.collider.gameObject == entity.gameObject) {
                        entity.Health.DoDamage(new Health.Damage() {
                            value = Mathf.CeilToInt(_damage * _damageCurve.Evaluate(Mathf.Min(1, Mathf.Max(0, vectorToCheckAngle.magnitude / (float)_raycastDistance)))),
                        });
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