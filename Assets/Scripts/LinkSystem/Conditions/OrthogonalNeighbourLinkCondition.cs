using AgaveLinkCase.Chip;
using AgaveLinkCase.GridSystem;
using UnityEngine;

namespace AgaveLinkCase.LinkSystem.Conditions
{
    [CreateAssetMenu(fileName = "OrthogonalNeighbourLinkCondition", menuName = "LinkConditions/OrthogonalNeighbour")]
    public class OrthogonalNeighbourLinkCondition : BaseLinkCondition
    {
        public override bool AreMet(ILinkable linkableA, ILinkable linkableB)
        {
            return GridUtils.IsOrthogonalNeighbor(linkableA.CellPos.x, linkableA.CellPos.y, linkableB.CellPos.x,
                linkableB.CellPos.y);
        }
    }
}