using System.Linq;
using UnityEngine;

namespace LinkSystem.Conditions
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