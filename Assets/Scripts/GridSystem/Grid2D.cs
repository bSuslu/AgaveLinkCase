using System;
using GridSystem.Converter;
using TMPro;
using UnityEngine;

namespace GridSystem
{
    public class Grid2D
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float CellSize { get; private set; }
        public Vector3 Origin { get; private set; }

        private Cell[,] _cells;

        private readonly CoordinateConverter _coordinateConverter;

        public event Action<int, int, Cell> OnValueChangeEvent;

        public Grid2D(int width, int height, float cellSize, Vector3 origin, CoordinateConverter converter,
            bool debug = false)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            Origin = origin;

            InitializeCells();

            _coordinateConverter = converter ?? new VerticalConverter();

            if (debug)
            {
                DrawDebugLines();
            }
        }

        private void InitializeCells()
        {
            _cells = new Cell[Width, Height];
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    _cells[x,y] = new Cell(x, y);
                }
            }
        }

        public void SetCell(Vector3 worldPosition, Cell cell)
        {
            Vector2Int cellIndex = _coordinateConverter.WorldToGridIndexes(worldPosition, CellSize, Origin);
            SetCell(cellIndex.x, cellIndex.y, cell);
        }

        public void SetCell(int x, int y, Cell cell)
        {
            if (IsValid(x, y))
            {
                _cells[x, y] = cell;
                OnValueChangeEvent?.Invoke(x, y, cell);
            }
        }

        public bool TryGetCell(Vector3 worldPosition, out Cell value)
        {
            Vector2Int cellIndex = _coordinateConverter.WorldToGridIndexes(worldPosition, CellSize, Origin);
            return TryGetCell(cellIndex.x, cellIndex.y, out value);
        }

        public bool TryGetCell(int x, int y, out Cell cell)
        {
            cell = null;
            if (IsValid(x, y))
            {
                cell = GetCell(x, y);
                return true;
            }

            return false;
        }

        public Cell GetCell(int x, int y)
        {
            return _cells[x, y];
        }
        
        public Cell GetCell(Vector2Int position) => GetCell(position.x, position.y);

        public bool IsValid(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

        public bool TryGetCellIndexes(Vector3 worldPosition, out Vector2Int cellIndex)
        {
            cellIndex = _coordinateConverter.WorldToGridIndexes(worldPosition, CellSize, Origin);
            return IsValid(cellIndex.x, cellIndex.y);
        }

        public Vector3 GetWorldPositionCenter(int x, int y) =>
            _coordinateConverter.GridToWorldCenter(x, y, CellSize, Origin);

        public Vector3 GetWorldPosition(int x, int y) => _coordinateConverter.GridToWorld(x, y, CellSize, Origin);


        private void DrawDebugLines()
        {
            const float duration = 100f;
            var parent = new GameObject("Debugging");

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    CreateWorldText(parent, x + "," + y, GetWorldPositionCenter(x, y), _coordinateConverter.Forward);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, duration);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, duration);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, Height), GetWorldPosition(Width, Height), Color.white, duration);
            Debug.DrawLine(GetWorldPosition(Width, 0), GetWorldPosition(Width, Height), Color.white, duration);
        }

        private TextMeshPro CreateWorldText(GameObject parent, string text, Vector3 position, Vector3 dir,
            int fontSize = 2, Color color = default, TextAlignmentOptions textAnchor = TextAlignmentOptions.Center,
            int sortingOrder = 0)
        {
            GameObject gameObject = new GameObject("DebugText_" + text, typeof(TextMeshPro));
            gameObject.transform.SetParent(parent.transform);
            gameObject.transform.position = position;
            gameObject.transform.forward = dir;

            TextMeshPro textMeshPro = gameObject.GetComponent<TextMeshPro>();
            textMeshPro.text = text;
            textMeshPro.fontSize = fontSize;
            textMeshPro.color = color == default ? Color.white : color;
            textMeshPro.alignment = textAnchor;
            textMeshPro.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

            return textMeshPro;
        }
    }
}