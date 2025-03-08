using Infrastructure;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class WinPopup : AbstractPopup {

        public struct Data {
            public int collectedMoney;
            public int totalLevelMoney;
            public Action menuButtonCallback;
            public Action continueButtonCallback;
        }

        [SerializeField]
        private TextMeshProUGUI _textLabel;

        [SerializeField]
        private LocalizationText _text;

        [SerializeField]
        private Button _menuButton;

        [SerializeField]
        private Button _continueButton;

        private Data _data;

        private void Awake() {
            _menuButton.onClick.AddListener(OnMenuButtonClicked);
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        public void SetData(Data data) {
            _data = data;
            /*_textLabel.text = string.Format(_text.GetText(), data.collectedMoney, data.totalLevelMoney,
               Mathf.Max(0, Account.Instance.RequiredMoney - Account.Instance.CurrentStageMoney - data.collectedMoney));*/
        }

        private void OnMenuButtonClicked() {
            _data.menuButtonCallback?.Invoke();
        }

        private void OnContinueButtonClicked() {
            HidePopup();
            _data.continueButtonCallback?.Invoke();
        }
    }
}

