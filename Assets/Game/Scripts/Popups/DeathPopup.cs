using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI {

    public class DeathPopup : AbstractPopup {

        [Serializable]
        public struct Data {
            public Action continueCallback;
            public Action respawnCallback;
        }

        [SerializeField]
        private Button _continueButton;

        [SerializeField]
        private Button _respawnButton;

        private Data _data;

        private void Awake() {
            _continueButton.onClick.AddListener(OnContinueButtonPressed);
            _respawnButton.onClick.AddListener(OnRespawnButtonClicked);
        }

        public void SetData(Data data) {
            _data = data;
        }

        private void OnRespawnButtonClicked() {
            YG2.RewardedAdvShow("Respawn", () => {
                _data.respawnCallback?.Invoke();
                YG2.MetricaSend("rewarded_adv","type", "respawn");
                HidePopup(immediately: true);
            });
        }

        private void OnContinueButtonPressed() {
            _data.continueCallback?.Invoke();
            HidePopup(immediately: true);
        }
    }
}
