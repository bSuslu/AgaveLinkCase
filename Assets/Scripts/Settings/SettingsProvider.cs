using UnityEngine;

namespace AgaveLinkCase.Settings
{
    public class SettingsProvider : MonoBehaviour
    {
        [field: SerializeField] public ChipSettings ChipSettings { get; private set; }
        [field: SerializeField] public UserLevelSettingsSliderLimitationSettings UserLevelSettingsSliderLimitationSettings { get; private set; }
    }
}