using AgaveLinkCase.Events;
using AgaveLinkCase.EventSystem;
using AgaveLinkCase.Level;
using AgaveLinkCase.Scene;
using AgaveLinkCase.ServiceLocatorSystem;
using AgaveLinkCase.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace AgaveLinkCase.UserLevelConfiguration
{
    public class UserLevelSettingsController : MonoBehaviour
    {
        [SerializeField] private UserLevelSettingSlider _targetScoreSlider;
        [SerializeField] private UserLevelSettingSlider _moveCountSlider;
        [SerializeField] private UserLevelSettingSlider _gridWidthSlider;
        [SerializeField] private UserLevelSettingSlider _gridHeightSlider;
 

        private void Start()
        {
            UserLevelConfigLimitationSettings settings = ServiceLocator.Global.Get<SettingsProvider>().UserLevelConfigLimitationSettings; // TODO: Remove>();
        
            _targetScoreSlider.SetSlider(settings.MinTargetScore, settings.MaxTargetScore, settings.AttributeNameTargetScore);
            _moveCountSlider.SetSlider(settings.MinMoveCount, settings.MaxMoveCount, settings.AttributeNameMoveCount);
            _gridWidthSlider.SetSlider(settings.MinGridWidth, settings.MaxGridWidth, settings.AttributeNameGridWidth);
            _gridHeightSlider.SetSlider(settings.MinGridHeight, settings.MaxGridHeight, settings.AttributeNameGridHeight);
        }
        
        private void OnButtonClicked()
        {
            LevelData levelData = new LevelData
            {
                TargetScore = _targetScoreSlider.GetSliderValue(),
                MoveCount = _moveCountSlider.GetSliderValue(),
                GridWidth = _gridWidthSlider.GetSliderValue(),
                GridHeight = _gridHeightSlider.GetSliderValue()
            };

            ServiceLocator.Global.Get<LevelService>().SetLevelData(levelData);
        }
    }
}
