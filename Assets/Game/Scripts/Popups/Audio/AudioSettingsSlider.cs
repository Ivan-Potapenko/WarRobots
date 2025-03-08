using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI {

    public class AudioSettingsSlider : MonoBehaviour {

        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private string _soundParameter;

        [SerializeField]
        private float _minVolumeValue;
        [SerializeField]
        private float _maxVolumeValue;

        [SerializeField]
        private AudioMixer _mixer;

        [SerializeField]
        private LocalizationText _titleText;

        [SerializeField]
        private TextMeshProUGUI _titleLabel;

        private void Awake() {
            _slider.onValueChanged.AddListener(UpdateVolume);
            UpdateSliderValue();
            _titleLabel.text = _titleText.GetText();
        }

        private void UpdateSliderValue() {
            _mixer.GetFloat(_soundParameter, out var volumeValue);
            _slider.SetValueWithoutNotify((volumeValue - _minVolumeValue) / (_maxVolumeValue - _minVolumeValue));
        }

        private void UpdateVolume(float value) {
            UpdateMixerVolume(_soundParameter, value);
        }

        private void UpdateMixerVolume(string mixerParams, float value) {
            _mixer.SetFloat(mixerParams, value * (_maxVolumeValue - _minVolumeValue) + _minVolumeValue);
        }
    }
}