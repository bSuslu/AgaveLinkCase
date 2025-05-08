using UnityEngine;

namespace AgaveLinkCase.Settings
{
    [CreateAssetMenu(fileName = "UserLevelConfigLimitationSettings", menuName = "Settings/UserLevelConfigLimitation")]
    public class UserLevelConfigLimitationSettings : ScriptableObject
    {
        [field: SerializeField] public int MinTargetScore { get; private set; }
        [field: SerializeField] public int MaxTargetScore { get; private set; }
        [field: SerializeField] public string AttributeNameTargetScore { get; private set; }
        [field: SerializeField] public int MinMoveCount { get; private set; }
        [field: SerializeField] public int MaxMoveCount { get; private set; }

        [field: SerializeField] public string AttributeNameMoveCount { get; private set; }
        [field: SerializeField] public int MinGridWidth { get; private set; }
        [field: SerializeField] public int MaxGridWidth { get; private set; }

        [field: SerializeField] public string AttributeNameGridWidth { get; private set; }
        [field: SerializeField] public int MinGridHeight { get; private set; }
        [field: SerializeField] public int MaxGridHeight { get; private set; }

        [field: SerializeField] public string AttributeNameGridHeight { get; private set; }
    }
}