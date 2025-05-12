using System;
using Chip;
using GridSystem;
using UnityEngine;

namespace LinkSystem.Validator
{
    public class LinkValidator : ILinkValidator
    {
        private readonly int _minLinkLength;
        private readonly Vector2Int[] _directionsToCheck;

        public LinkValidator(int minLinkLength, Vector2Int[] directions)
        {
            _minLinkLength = minLinkLength;
            _directionsToCheck = directions;
        }

        public bool IsLinkExist(Grid2D grid2D)
        {
            return HasChainOfLength(grid2D.Cells, _minLinkLength); 
        }
        
        private Grid2D _grid2D;
        public bool HasChainOfLength(Cell[,] matrix, int minLength)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            bool[,] visited = new bool[rows, cols];
            int maxLength = 0;

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    Array.Clear(visited, 0, visited.Length);
                    visited[x, y] = true;
                    int length = DFS(matrix, visited, x, y, matrix[x, y].ChipEntity.Type);
                    maxLength = Math.Max(maxLength, length);
                    if (maxLength >= minLength)
                        return true;
                }
            }

            return false;
        }

        private int DFS(Cell[,] matrix, bool[,] visited, int x, int y, ChipType target)
        {
            int maxLen = 1;

            foreach (var dir in _directionsToCheck)
            {
                int nx = x + dir.x;
                int ny = y + dir.y;

                if (IsValid(matrix, nx, ny) && !visited[nx, ny] && matrix[nx, ny].ChipEntity.Type == target)
                {
                    visited[nx, ny] = true;
                    int len = 1 + DFS(matrix, visited, nx, ny, target);
                    visited[nx, ny] = false;
                    maxLen = Math.Max(maxLen, len);
                }
            }

            return maxLen;
        }

        private bool IsValid(Cell[,] matrix, int x, int y)
        {
            return x >= 0 && x < matrix.GetLength(0) && y >= 0 && y < matrix.GetLength(1);
        }
    }
}