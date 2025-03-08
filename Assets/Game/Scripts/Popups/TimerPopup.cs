using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI {

    public class TimerPopup : AbstractPopup {

        public struct Data {
            public int time;
            public Action onTimerEnd;
            public string additionalText;
        }

        [SerializeField]
        private TextMeshProUGUI _timeText;

        private WaitForSeconds _waitForSecond = new WaitForSeconds(1);

        private Data _data;

        private Coroutine _coroutine;

        public void SetData(Data data) {
            _data = data;
        }

        public override void ShowPopup() {
            base.ShowPopup();
            if (_coroutine != null) {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(TimerCoroutine());
        }

        private IEnumerator TimerCoroutine() {
            var seconds = _data.time;
            _timeText.text = string.IsNullOrEmpty(_data.additionalText) ? seconds.ToString() : string.Format(_data.additionalText, seconds);
            while (seconds > 0) {
                yield return _waitForSecond;
                seconds--;
                _timeText.text = string.IsNullOrEmpty(_data.additionalText) ? seconds.ToString() : string.Format(_data.additionalText, seconds);
            }
            _data.onTimerEnd?.Invoke();
            HidePopup();
        }

        public override void HidePopup(bool immediately = false) {
            base.HidePopup(immediately);
            if (_coroutine != null) {
                StopCoroutine(_coroutine);
            }
        }
    }
}