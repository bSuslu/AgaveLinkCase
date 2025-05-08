using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AgaveLinkCase.UserLevelConfiguration
{
    public class UserLevelSettingSlider : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _labelText;
        [SerializeField] private Slider _slider;

        private string _levelConfigAttributeName;

        private void Awake()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        public void SetSlider(float min, float max, string levelConfigAttributeName)
        {
            _slider.minValue = min;
            _slider.maxValue = max;
            _levelConfigAttributeName = levelConfigAttributeName;
        
            _slider.value = min;
            UpdateSliderValueText(_slider.value);
        }

        private void OnSliderValueChanged(float sliderValue)
        {
            UpdateSliderValueText(sliderValue);
        }
    
        private void UpdateSliderValueText(float sliderValue)
        {
            _labelText.text = $" {_levelConfigAttributeName}: {Mathf.RoundToInt(sliderValue)}";
        }
        
        public int GetSliderValue()
        {
            return Mathf.RoundToInt(_slider.value);
        }
    }
}
