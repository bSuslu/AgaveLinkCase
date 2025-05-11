using Chip;
using UnityEngine;

namespace GridSystem
{
    public class Cell
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public ChipEntity ChipEntity { get; private set; }

        public bool IsLocked { get; set; }
        public bool IsOccupied => ChipEntity != null;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void DestroyChip()
        {
            ChipEntity.Reset();
            ChipEntity = null;
        }

        public void SetChip(ChipEntity newChip)
        {
            if (ChipEntity != null && newChip != null)
            {
                Debug.LogError("Cell.SetChip: ChipEntity already set");
            }
            
            if (newChip != null)
                newChip.CellPos = new Vector2Int(X, Y);
            
            ChipEntity = newChip;
        }

        public void Lock(bool isLocked = true)
        {
            IsLocked = isLocked;
        }
    }
}