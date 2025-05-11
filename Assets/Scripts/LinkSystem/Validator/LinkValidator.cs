using System.Collections.Generic;
using System.Linq;
using AgaveLinkCase.GridSystem;
using AgaveLinkCase.LinkSystem.Conditions;
using UnityEngine;

namespace AgaveLinkCase.LinkSystem.Validator
{
    public class LinkValidator : ILinkValidator
    {
        private readonly int _minLinkLength;
        private readonly List<LinkCondition> _conditions;
        private readonly Vector2Int[] _neighborDirectionsToCheck;

        public LinkValidator(List<LinkCondition> conditions, int minLinkLength, Vector2Int[] directions)
        {
            _conditions = conditions;
            _minLinkLength = minLinkLength;
            _neighborDirectionsToCheck = directions;
        }

        public bool IsLinkExist(Grid2D grid2D)
        {
            int width = grid2D.Width;
            int height = grid2D.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var position = new Vector2Int(x, y);
                    var linkables = GetLinkChain(grid2D, position);

                    if (linkables.Count >= _minLinkLength)
                    {
                        // string linkablePositions = string.Join(", ", linkables.Select(l => l.CellPos));
                        // Debug.Log($"Link found: {linkablePositions}");
                        return true;
                    }
                }
            }

            return false;
        }

        private List<ILinkable> GetLinkChain(Grid2D grid, Vector2Int startCoordinate)
        {
            var result = new List<ILinkable>();
            var visited = new HashSet<Vector2Int>();

            if (!grid.TryGetCell(startCoordinate.x, startCoordinate.y, out var startCell) ||
                !startCell.IsOccupied || startCell.ChipEntity is not ILinkable startLinkable)
                return result;

            bool IsValidLink(Vector2Int from, Vector2Int to, out ILinkable toLinkable)
            {
                toLinkable = null;
                if (!grid.TryGetCell(to.x, to.y, out var cell)) return false;
                if (!cell.IsOccupied || cell.ChipEntity is not ILinkable nextLinkable) return false;
                if (!_conditions.All(c => c.AreMet(grid.GetCell(from).ChipEntity as ILinkable, nextLinkable))) return false;
                toLinkable = nextLinkable;
                return true;
            }

            bool DFS(Vector2Int current, Vector2Int? previous)
            {
                visited.Add(current);
                var currentLinkable = grid.GetCell(current).ChipEntity as ILinkable;
                result.Add(currentLinkable);

                int validNeighborCount = 0;
                foreach (var dir in _neighborDirectionsToCheck)
                {
                    var next = current + dir;
                    if (previous.HasValue && next == previous.Value) continue;
                    if (visited.Contains(next)) continue;

                    if (IsValidLink(current, next, out var _))
                    {
                        validNeighborCount++;
                        if (validNeighborCount > 1) return false; // branching
                        if (!DFS(next, current)) return false;
                    }
                }

                return true;
            }

            if (!DFS(startCoordinate, null) || result.Count < _minLinkLength)
                return new List<ILinkable>();

            return result;
        }
    }
}