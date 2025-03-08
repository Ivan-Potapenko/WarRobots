using System;
using TMPro;
using UnityEngine;

namespace UI {

    public class TextPopup : AbstractTemporaryPopup {

        public enum Type {
            Upper,
            Middle,
            MiddleBig,
        }

        public struct Data {
            public string text;
            public Type type;
            public Action onPopupShowedCallback;
            public float time;
        }

        [Serializable]
        private class TextByType {
            public Type type;
            public GameObject textGroup;
            public TextMeshProUGUI text;
        }

        [SerializeField]
        private TextByType[] _texts;

        private Data _data;

        public void SetData(Data data) {
            _data = data;
            _overrideShowTime = _data.time;
            DisableTexts();
            foreach (var textWithType in _texts) {
                if(data.type == textWithType.type) {
                    textWithType.textGroup.SetActive(true);
                    textWithType.text.text = data.text;
                }
            }
        }

        protected override void OnPopupShowed() {
            base.OnPopupShowed();
            _data.onPopupShowedCallback?.Invoke();
        }

        private void DisableTexts() {
            foreach (var text in _texts) {
                text.textGroup.SetActive(false);
            }
        }
    }
}
