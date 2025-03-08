using Infrastructure;
using System;
using UnityEngine;

namespace UI {

    public class PopupManager : SingletonCrossScene<PopupManager> {
/*
        [SerializeField]
        private TextPopup _textPopupPrefab;
        private TextPopup _textPopup;

        [SerializeField]
        private DeathPopup _deathPopupPrefab;
        private DeathPopup _deathPopup;

        [SerializeField]
        private FadePopup _fadePopupPrefab;
        private FadePopup _fadePopup;

        [SerializeField]
        private WinPopup _winPopupPrefab;
        private WinPopup _winPopup;

        [SerializeField]
        private TimerPopup _timerPopupPrefab;
        private TimerPopup _timerPopup;

        [SerializeField]
        private ClosableTextPopup _closableTextPopupPrefab;
        private ClosableTextPopup _closableTextPopup;

        [SerializeField]
        private AudioSettingsPopup _audioSettingsPopupPrefab;
        private AudioSettingsPopup _audioSettingsPopup;

        [SerializeField]
        private BookPopup _bookPopupPrefab;
        private BookPopup _bookPopup;

        [SerializeField]
        private AudioSource _popupSound;

        [SerializeField]
        private StartBonusPopup _startBonusPopupPrefab;
        private StartBonusPopup _startBonusPopup;

        private AbstractPopup _currentPopup;

        private void ShowPopup(AbstractPopup popup) {
            CloseCurrentPopup(immediately: true);
            _currentPopup = popup;
            popup.ShowPopup();
        }

        public void ShowTextPopup(TextPopup.Data data) {
            if (_textPopup == null) {
                _textPopup = Instantiate(_textPopupPrefab, gameObject.transform);
            }
            _textPopup.SetData(data);
            ShowPopup(_textPopup);
        }

        public void ShowDeathPopup(DeathPopup.Data data) {
            if (_deathPopup == null) {
                _deathPopup = Instantiate(_deathPopupPrefab, gameObject.transform);
            }
            _deathPopup.SetData(data);
            ShowPopup(_deathPopup);
        }

        public void ShowFadePopup(Action onShowedCallback) {
            if (_fadePopup == null) {
                _fadePopup = Instantiate(_fadePopupPrefab, gameObject.transform);
            }
            _fadePopup.SetData(onShowedCallback);
            ShowPopup(_fadePopup);
        }

        public void ShowWinPopup(WinPopup.Data data) {
            if (_winPopup == null) {
                _winPopup = Instantiate(_winPopupPrefab, gameObject.transform);
            }
            _winPopup.SetData(data);
            ShowPopup(_winPopup);
        }

        public void ShowTimerPopup(TimerPopup.Data data) {
            if (_timerPopup == null) {
                _timerPopup = Instantiate(_timerPopupPrefab, gameObject.transform);
            }
            _timerPopup.SetData(data);
            ShowPopup(_timerPopup);
        }

        public void ShowClosableTextPopup(ClosableTextPopup.Data data) {
            if (_closableTextPopup == null) {
                _closableTextPopup = Instantiate(_closableTextPopupPrefab, gameObject.transform);
            }
            _closableTextPopup.SetData(data);
            ShowPopup(_closableTextPopup);
        }

        public void ShowAudioSettingsPopup() {
            if (_audioSettingsPopup == null) {
                _audioSettingsPopup = Instantiate(_audioSettingsPopupPrefab, gameObject.transform);
            }
            ShowPopup(_audioSettingsPopup);
        }

        public void ShowBookPopup(Enemy.EnemyType enemyType = Enemy.EnemyType.None) {
            if (_bookPopup == null) {
                _bookPopup = Instantiate(_bookPopupPrefab, gameObject.transform);
            }
            _bookPopup.SetData(enemyType);
            ShowPopup(_bookPopup);
        }

        public void ShowStartBonusPopup(StartBonusPopup.Data data) {
            if (_startBonusPopup == null) {
                _startBonusPopup = Instantiate(_startBonusPopupPrefab, gameObject.transform);
            }
            _startBonusPopup.SetData(data);
            _startBonusPopup.ShowPopup();
        }

        public void CloseCurrentPopup(bool immediately) {
            if (_currentPopup != null && _currentPopup.IsActive) {
                _currentPopup.HidePopup(immediately);
            }
        }

        public void PlayPopupSound() {
            if(_popupSound == null) {
                return;
            }
            _popupSound.Play();
        }*/
    }
}
