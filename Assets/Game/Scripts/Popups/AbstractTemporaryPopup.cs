using System.Collections;
using UnityEngine;

namespace UI {

    public class AbstractTemporaryPopup : AbstractPopup {

        [SerializeField]
        private float _showTime;
        protected float _overrideShowTime;

        private Coroutine _hideCoroutine;

        protected override void OnPopupShowed() {
            base.OnPopupShowed();
            _hideCoroutine = StartCoroutine(HideCoroutine());
        }

        public override void HidePopup(bool immediately = false) {
            if (_hideCoroutine != null) {
                StopCoroutine(_hideCoroutine);
            }
            base.HidePopup(immediately);
        }

        private IEnumerator HideCoroutine() {
            yield return new WaitForSeconds(_overrideShowTime != 0 ? _overrideShowTime : _showTime);
            HidePopup();
        }
    }
}
