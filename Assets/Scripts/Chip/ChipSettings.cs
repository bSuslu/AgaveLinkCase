using AgaveLinkCase.Chip;
using UnityEngine;

namespace AgaveLinkCase.Settings
{
    [CreateAssetMenu(fileName = "ChipSettings", menuName = "Settings/Chip")]
    public class ChipSettings : ScriptableObject
    {
        [field: SerializeField] public ChipConfig[] ChipConfigs { get; private set; }
    }
}