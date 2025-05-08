using AgaveLinkCase.Chip;
using AgaveLinkCase.GridSystem;
using UnityEngine;

namespace AgaveLinkCase.Settings
{
    public class SettingsProvider : MonoBehaviour
    {
        [field: SerializeField] public ChipSettings ChipSettings { get; private set; }
        [field: SerializeField] public GridSettings GridSettings { get; private set; }
        [field: SerializeField] public UserLevelConfigLimitationSettings UserLevelConfigLimitationSettings { get; private set; }
    }
}