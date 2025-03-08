using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class ClosablePopup : AbstractPopup {

        [SerializeField]
        private Button _closeButton;

        protected virtual void Awake() {
            _closeButton.onClick.AddListener(() => HidePopup());
        }

        public override void ShowPopup() {
            base.ShowPopup();
           // PopupManager.Instance.PlayPopupSound();
        }

        public override void HidePopup(bool immediately = false) {
            base.HidePopup(immediately);
           // PopupManager.Instance.PlayPopupSound();
        }
    }
}