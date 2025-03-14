using Game;
using UnityEngine;

namespace Data {

    [CreateAssetMenu(fileName = nameof(MechData), menuName = "Data/Game/MechData")]
    public class MechData : ScriptableObject {

        [SerializeField]
        private Character _characterPrefab;
        public Character Character => _characterPrefab;
    }
}