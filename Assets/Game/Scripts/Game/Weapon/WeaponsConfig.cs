using UnityEngine;

namespace Data {

    [CreateAssetMenu(fileName = nameof(WeaponsConfig), menuName = "Data/Game/WeaponsConfig")]
    public class WeaponsConfig : ScriptableObject {

        [SerializeField]
        private WeaponData[] _weaponsData;
        public WeaponData[] WeaponsData => _weaponsData;
    }
}