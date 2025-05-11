using UnityEngine;

namespace UserLevelConfiguration
{
    [CreateAssetMenu(fileName = "UserLevelConfigLimitationSettings", menuName = "Settings/UserLevelConfigLimitation")]
    public class UserLevelConfigLimitationSettings : ScriptableObject
    {
        [field: SerializeField] public int MinTargetScore { get; private set; }
        [field: SerializeField] public int MaxTargetScore { get; private set; }
        [field: SerializeField] public string LabelTargetScore { get; private set; }
        [field: SerializeField] public int MinMoveCount { get; private set; }
        [field: SerializeField] public int MaxMoveCount { get; private set; }

        [field: SerializeField] public string LabelMoveCount { get; private set; }
        [field: SerializeField] public int MinGridWidth { get; private set; }
        [field: SerializeField] public int MaxGridWidth { get; private set; }

        [field: SerializeField] public string LabelGridWidth { get; private set; }
        [field: SerializeField] public int MinGridHeight { get; private set; }
        [field: SerializeField] public int MaxGridHeight { get; private set; }

        [field: SerializeField] public string LabelGridHeight { get; private set; }
    }
}