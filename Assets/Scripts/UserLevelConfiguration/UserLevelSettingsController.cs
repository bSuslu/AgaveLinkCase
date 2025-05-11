using EventSystem;
using LevelSystem;
using Scene;
using ServiceLocatorSystem;
using Settings;
using UnityEngine;

namespace UserLevelConfiguration
{
    public class UserLevelSettingsController : MonoBehaviour
    {
        [SerializeField] private UserLevelSettingSlider _targetScoreSlider;
        [SerializeField] private UserLevelSettingSlider _moveCountSlider;
        [SerializeField] private UserLevelSettingSlider _gridWidthSlider;
        [SerializeField] private UserLevelSettingSlider _gridHeightSlider;
 
        private EventBinding<OnBeforeSceneUnloadEvent> _loadSceneRequestEventBinding;

        private void Awake()
        {
            _loadSceneRequestEventBinding = new EventBinding<OnBeforeSceneUnloadEvent>(OnBeforeSceneUnload);
            EventBus<OnBeforeSceneUnloadEvent>.Subscribe(_loadSceneRequestEventBinding);
            
        }

        private void OnDestroy()
        {
            EventBus<OnBeforeSceneUnloadEvent>.Unsubscribe(_loadSceneRequestEventBinding);
        }

        private void Start()
        {
            UserLevelConfigLimitationSettings settings = ServiceLocator.Global.Get<SettingsProvider>().UserLevelConfigLimitationSettings;
        
            _targetScoreSlider.SetSlider(settings.MinTargetScore, settings.MaxTargetScore, settings.LabelTargetScore);
            _moveCountSlider.SetSlider(settings.MinMoveCount, settings.MaxMoveCount, settings.LabelMoveCount);
            _gridWidthSlider.SetSlider(settings.MinGridWidth, settings.MaxGridWidth, settings.LabelGridWidth);
            _gridHeightSlider.SetSlider(settings.MinGridHeight, settings.MaxGridHeight, settings.LabelGridHeight);
        }
        
        private void OnBeforeSceneUnload()
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
