using TMPro;
using UnityEngine;

namespace Infrastructure {

    public class TMPTextSetter : MonoBehaviour {
        [SerializeField]
        private TMP_Text _label;

        [SerializeField]
        private LocalizationText _localization;

        private void Awake() {
            _label.text = _localization.GetText();
        }
    }
}
