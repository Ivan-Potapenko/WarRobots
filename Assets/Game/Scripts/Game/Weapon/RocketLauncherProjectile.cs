using UnityEngine;

namespace Game
{

    public class RocketLauncherProjectile : AbstractProjectile
    {
        [SerializeField]
        private int homingStrength;

        private Vector3 direction;
        private Transform _target;
        void Start()
        {
            direction = transform.forward;
        }
        private void Update()
        {
            DestructionThroughTime();
            if (_target != null)
            {
                Vector3 toTarget = new Vector3(_target.position.x - transform.position.x, 0, _target.position.z - transform.position.z).normalized;
                direction = Vector3.Slerp(direction, toTarget, homingStrength * Time.deltaTime);
            }
            transform.position += direction * Speed * Time.deltaTime;
            transform.forward = direction;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<Health>(out var health))
            {
                _target = other.transform;
            }
        }
    }
}
