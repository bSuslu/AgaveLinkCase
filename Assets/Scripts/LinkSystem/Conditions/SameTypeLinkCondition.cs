using UnityEngine;

namespace LinkSystem.Conditions
{
    [CreateAssetMenu(fileName = "SameTypeLinkCondition", menuName = "LinkConditions/SameType")]
    public class SameTypeLinkCondition : LinkCondition
    {
        public override bool AreMet(ILinkable linkableA, ILinkable linkableB)
        {
            if (linkableA.CanBeLinkedWith(linkableB))
                return true;
            return false;
        }
    }
}