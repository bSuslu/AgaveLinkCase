using AgaveLinkCase.UserLevelConfiguration;
using UnityEngine;

namespace AgaveLinkCase.Settings
{
    
    [CreateAssetMenu(fileName = "UserLevelSettingsSliderLimitationSettings", menuName = "Settings/UserLevelSettingsSliderLimitation")]
    public class UserLevelSettingsSliderLimitationSettings : ScriptableObject
    {
        [field: SerializeField] public SliderLimitationConfig[] SliderLimitations { get; private set; }
    }
}