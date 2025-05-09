using AgaveLinkCase.LinkSystem.Conditions;
using UnityEngine;

namespace AgaveLinkCase.LinkSystem
{
    [CreateAssetMenu(fileName = "LinkSettings", menuName = "Settings/Link")]
    public class LinkSettings : ScriptableObject
    {
        [field: SerializeField] public BaseLinkCondition[] LinkConditions { get; private set; }
        [field: SerializeField] public int MinLinkLength { get; private set; }
        
    }
}
