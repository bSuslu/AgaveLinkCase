using UnityEngine;

namespace AgaveLinkCase.GridSystem.Converter
{
    public abstract class CoordinateConverter
    {
        public abstract Vector3 GridToWorld(int gridX, int gridY, float cellSize, Vector3 originPosition);
        public abstract Vector3 GridToWorldCenter(int gridX, int gridY, float cellSize, Vector3 originPosition);
        public abstract Vector2Int WorldToGridIndexes(Vector3 worldPosition, float cellSize, Vector3 originPosition);
        public abstract Vector3 Forward { get; }
    }
}