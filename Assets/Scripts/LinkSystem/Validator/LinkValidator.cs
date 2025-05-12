using System;
using System.Collections.Generic;
using System.Linq;
using Chip;
using GridSystem;
using LinkSystem.Conditions;
using UnityEngine;

namespace LinkSystem.Validator
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
            return HasChainOfLenght(grid2D, _minLinkLength); 
        }
        
        private Grid2D _grid2D;
        private bool HasChainOfLenght(Grid2D grid2D, int minLinkLength)
        {
            _grid2D = grid2D;
            bool [,] visited = new bool[grid2D.Width, grid2D.Height];

            for (int i = 0; i < grid2D.Width; i++)
            {
                for (int j = 0; j < grid2D.Height; j++)
                {
                    visited[i, j] = true;
                    int length = DFS(i, j, grid2D.Cells[i, j].ChipEntity.Type, visited);
                    if (length >= minLinkLength)
                        return true;
                    visited[i, j] = false;
                }
            }
            return false; 
        }
        
        private int DFS(int x, int y, ChipType targetChipType, bool[,] visited)
        {
            int length = 1;
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            for (int dir = 0; dir < 4; dir++)
            {
                int nx = x + dx[dir];
                int ny = y + dy[dir];

                if (IsValid(nx, ny) && !visited[nx, ny] && _grid2D.Cells[nx, ny].ChipEntity.Type == targetChipType)
                {
                    visited[nx, ny] = true;
                    length = Math.Max(length, 1 + DFS(nx, ny, targetChipType, visited));
                    visited[nx, ny] = false;
                }
            }

            return length;
        }
        
        private bool IsValid(int x, int y)
        {
            return x >= 0 && x < _grid2D.Width && y >= 0 && y < _grid2D.Height;
        }
    }
}