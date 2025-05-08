
using UnityEngine;

namespace AgaveLinkCase.UserLevelConfiguration
{
    [CreateAssetMenu(fileName = "SliderLimitationConfig", menuName = "Config/SliderLimitation")]
    public class SliderLimitationConfig : ScriptableObject
    {
        [field: SerializeField] public int Min { get; private set; }
        [field: SerializeField] public int Max { get; private set; }
        [field: SerializeField] public string Label { get; private set; }
    }
}
