using UnityEngine;

namespace Game {

    public class ProjectileWeapon : AbstractWeapon {

        [SerializeField]
        private AbstractProjectile _abstractProjectilePrefab;// ���� ������ ����

        protected override void ShootInternal() {
            // ��� ������ ���� � ����� ShootPoint
        }
    }
}
