using UnityEngine;

namespace AgaveLinkCase.GridSystem
{
    // TODO : Check abstraction if neccessary
    public class VerticalConverter : CoordinateConverter
    {
        public override Vector3 GridToWorld(int gridX, int gridY, float cellSize, Vector3 originPosition)
        {
            return new Vector3(gridX, gridY, 0) * cellSize + originPosition;
        }

        public override Vector3 GridToWorldCenter(int gridX, int gridY, float cellSize, Vector3 originPosition)
        {
            return new Vector3(gridX * cellSize + cellSize * 0.5f, gridY * cellSize + cellSize * 0.5f, 0) +
                   originPosition;
        }

        public override Vector2Int WorldToGridIndexes(Vector3 worldPosition, float cellSize, Vector3 originPosition)
        {
            Vector3 relativePosition = (worldPosition - originPosition) / cellSize;
            var gridX = Mathf.FloorToInt(relativePosition.x);
            var gridY = Mathf.FloorToInt(relativePosition.y);
            return new Vector2Int(gridX, gridY);
        }

        public override Vector3 Forward => Vector3.forward;
    }
}