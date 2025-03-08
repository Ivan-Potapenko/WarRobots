using System;
using TMPro;
using UnityEngine;

namespace UI {

    public class ClosableTextPopup : ClosablePopup {

        public struct Data {
            public string text;
            public Action onPopupClosed;
        }

        [SerializeField]
        private TextMeshProUGUI _textLabel;

        private Data _data;

        public void SetData(Data data) {
            _data = data;
            _textLabel.text = data.text;
        }

        protected override void OnPopupClosed() {
            base.OnPopupClosed();
            _data.onPopupClosed?.Invoke();
        }
    }
}