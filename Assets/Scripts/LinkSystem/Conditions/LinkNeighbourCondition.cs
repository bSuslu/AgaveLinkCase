using System.Linq;
using AgaveLinkCase.Chip;
using UnityEngine;

namespace AgaveLinkCase.LinkSystem.Conditions
{
    [CreateAssetMenu(fileName = "LinkNeighbourCondition", menuName = "LinkConditions/Neighbour")]
    public class LinkNeighbourCondition : LinkCondition
    {
        [field: SerializeField] public Vector2Int[] Directions { get; private set; }

        public override bool AreMet(ILinkable linkableA, ILinkable linkableB)
        {
            return Directions.Contains(linkableA.CellPos - linkableB.CellPos);
        }
    }
}