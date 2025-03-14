using Game;
using UnityEngine;

namespace Data {

    [CreateAssetMenu(fileName = nameof(WeaponData), menuName = "Data/Game/WeaponData")]
    public class WeaponData : ScriptableObject {

        [SerializeField]
        private AbstractWeapon _weaponPrefab;
        public AbstractWeapon WeaponPrefab => _weaponPrefab;
    }
}