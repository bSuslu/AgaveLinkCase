using AgaveLinkCase.Chip;
using AgaveLinkCase.GridSystem;
using UnityEngine;

namespace AgaveLinkCase.LinkSystem.Conditions
{
    [CreateAssetMenu(fileName = "DiagonalNeighbourCondition", menuName = "LinkConditions/DiagonalNeighbour")]
    public class DiagonalNeighbourCondition : BaseLinkCondition
    {
        public override bool AreMet(ILinkable linkableA, ILinkable linkableB)
        {
            return GridUtils.IsDiagonalNeighbor(linkableA.CellPos.x, linkableA.CellPos.y, linkableB.CellPos.x,
                linkableB.CellPos.y);
        }
        
    }
}