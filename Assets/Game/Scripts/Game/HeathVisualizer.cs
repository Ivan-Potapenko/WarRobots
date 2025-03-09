using UnityEngine;

namespace Game {

    public class HeathVisualizer : MonoBehaviour {

        [SerializeField]
        private GameObject _healthToResize;

        [SerializeField]
        private GameObject _healthBack;

        private int _startHealth;

        private Health _health;

        public void Init(Health health) {
            _health = health;
            _startHealth = health.Value;
            health.onValueChanged += OnHealthChanged;
        }

        private void OnHealthChanged() {
            if(_health.Value <= 0) {
                gameObject.SetActive(false);
                return;
            }
            var size = Mathf.Lerp(0, _healthBack.transform.localScale.x, (float)_health.Value / _startHealth);
            var newScale = new Vector3(size, _healthToResize.transform.localScale.y, _healthToResize.transform.localScale.z);
            _healthToResize.transform.localPosition =
                new Vector3(_healthBack.transform.localPosition.x - (_healthBack.transform.localScale.x - newScale.x) / 2, _healthBack.transform.localPosition.y, _healthBack.transform.localPosition.z);
            _healthToResize.transform.localScale = newScale;
        }
    }
}
