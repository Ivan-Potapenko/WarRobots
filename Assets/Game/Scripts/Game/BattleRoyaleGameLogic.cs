using UnityEngine;

namespace Game {

    public class BattleRoyaleGameLogic : MonoBehaviour {

        [SerializeField]
        private Zone _zone;
        public Zone Zone => _zone;

        [SerializeField]
        private int _damage;

        [SerializeField]
        private float _timeBetweenDoDamage;

        private float _currentTimeBeforeDamage;

        public void Init() {
            _zone.Init();
        }

        public void UpdateLogic() {
            if(_currentTimeBeforeDamage > 0) {
                _currentTimeBeforeDamage -= Time.deltaTime;
                return;
            }
            _currentTimeBeforeDamage = _timeBetweenDoDamage;
            foreach (var character in BattleManager.instance.Characters) {
                if(character.Health.CurrentState == Health.State.Death || _zone.InSafeZone(character.transform.position)) {
                    continue;
                }
                character.Health.DoDamage(new Health.Damage() {
                    value = _damage,
                });
            }
        }
    }
}