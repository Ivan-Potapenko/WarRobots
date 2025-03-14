using UnityEngine;

namespace Game {

    public class ProjectileWeapon : AbstractWeapon {

        [SerializeField]
        private AbstractProjectile _abstractProjectilePrefab;// —юда префаб пули

        protected override void ShootInternal() {
            // “ут создаЄм пулю в точке ShootPoint
        }
    }
}
