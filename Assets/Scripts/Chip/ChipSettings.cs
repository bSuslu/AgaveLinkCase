using UnityEngine;

namespace Chip
{
    [CreateAssetMenu(fileName = "ChipSettings", menuName = "Settings/Chip")]
    public class ChipSettings : ScriptableObject
    {
        [field: SerializeField] public ChipConfig[] ChipConfigs { get; private set; }
        [field: SerializeField] public ChipEntity ChipEntityPrefab { get; private set; }
    }
}