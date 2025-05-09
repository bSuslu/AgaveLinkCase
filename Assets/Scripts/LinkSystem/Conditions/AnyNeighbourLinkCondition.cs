using AgaveLinkCase.Chip;
using AgaveLinkCase.GridSystem;
using UnityEngine;

namespace AgaveLinkCase.LinkSystem.Conditions
{
    [CreateAssetMenu(fileName = "AnyNeighbourLinkCondition", menuName = "LinkConditions/AnyNeighbour")]
    public class AnyNeighbourLinkCondition : BaseLinkCondition
    {
        public override bool AreMet(ILinkable linkableA, ILinkable linkableB)
        {
            return GridUtils.IsAnyNeighbor(linkableA.CellPos.x, linkableA.CellPos.y, linkableB.CellPos.x,
                linkableB.CellPos.y);
        }
    }
}