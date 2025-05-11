using AgaveLinkCase.GridSystem.Converter;
using AgaveLinkCase.LevelSystem;
using UnityEngine;

namespace AgaveLinkCase.GridSystem
{
    public class GridFactory
    {
        public Grid2D Create(GridSettings gridSettings,LevelData levelData)
        {
            int width = levelData.GridWidth;
            int height = levelData.GridHeight;
            
            float cellSize = gridSettings.CellSize;
            Vector3 originPosition = gridSettings.OriginPosition;
            bool debug = gridSettings.Debug;
            
            Grid2D grid = new Grid2D(width, height, cellSize, originPosition, new VerticalConverter(), debug);
            
            return grid;
        }
    }
}