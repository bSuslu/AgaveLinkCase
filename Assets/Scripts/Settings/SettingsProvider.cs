using AgaveLinkCase.Chip;
using AgaveLinkCase.GridSystem;
using AgaveLinkCase.LinkSystem;
using AgaveLinkCase.UserLevelConfiguration;
using UnityEngine;

namespace AgaveLinkCase.Settings
{
    public class SettingsProvider : MonoBehaviour
    {
        [field: SerializeField] public ChipSettings ChipSettings { get; private set; }
        [field: SerializeField] public GridSettings GridSettings { get; private set; }
        [field: SerializeField] public UserLevelConfigLimitationSettings UserLevelConfigLimitationSettings { get; private set; }
        [field: SerializeField] public LinkSettings LinkSettings { get; private set; }
        [field: SerializeField] public VisualSettings VisualSettings { get; private set; }

    }
}