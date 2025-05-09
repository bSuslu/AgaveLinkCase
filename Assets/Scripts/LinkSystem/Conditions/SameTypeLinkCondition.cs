using AgaveLinkCase.Chip;
using UnityEngine;

namespace AgaveLinkCase.LinkSystem.Conditions
{
    [CreateAssetMenu(fileName = "SameTypeLinkCondition", menuName = "LinkConditions/SameType")]
    public class SameTypeLinkCondition : BaseLinkCondition
    {
        public override bool AreMet(ILinkable linkableA, ILinkable linkableB)
        {
            if (linkableA.CanBeLinkedWith(linkableB))
                return true;
            return false;
        }
    }
}