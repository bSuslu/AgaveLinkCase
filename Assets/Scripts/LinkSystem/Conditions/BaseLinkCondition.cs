using AgaveLinkCase.Chip;
using UnityEngine;

namespace AgaveLinkCase.LinkSystem.Conditions
{
    public abstract class BaseLinkCondition : ScriptableObject
    {
        public abstract bool AreMet(ILinkable linkableA, ILinkable linkableB);
    }
}