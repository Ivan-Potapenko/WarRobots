using UnityEngine;

namespace Game
{

    public abstract class AbstractProjectile : MonoBehaviour
    {
        [SerializeField]
        private int _damage;
        [SerializeField]
        private int _speed;
        [SerializeField]
        private GameObject _destroyEffect;
        [SerializeField]
        private float _lifetime = 2f;

        public int Damage => _damage;
        public int Speed => _speed;
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<Health>(out var health))
            {
                health.DoDamage(new Health.Damage() { value = _damage });
            }
            Instantiate(_destroyEffect, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        public void DestructionThroughTime()
        {
            _lifetime -= Time.deltaTime;
            if (_lifetime < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
