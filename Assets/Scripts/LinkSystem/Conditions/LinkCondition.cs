using UnityEngine;

namespace LinkSystem.Conditions
{
    public abstract class LinkCondition : ScriptableObject
    {
        public abstract bool AreMet(ILinkable linkableA, ILinkable linkableB);
    }
}