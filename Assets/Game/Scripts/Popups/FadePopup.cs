using System;

namespace UI {

    public class FadePopup : AbstractTemporaryPopup {

        private Action _onShowedCallback;

        public void SetData(Action onShowedCallback) {
            _onShowedCallback = onShowedCallback;
        }

        protected override void OnPopupShowed() {
            base.OnPopupShowed();
            _onShowedCallback();
        }
    }
}

