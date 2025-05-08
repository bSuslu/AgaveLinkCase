using AgaveLinkCase.Chip;
using UnityEngine;

namespace AgaveLinkCase.GridSystem
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

        public void SetChip(ChipEntity chipEntity)
        {
            if (ChipEntity != null && chipEntity != null)
            {
                Debug.LogError("Cell.SetChip: ChipEntity already set");
            }

            ChipEntity = chipEntity;
        }

        public void Lock(bool isLocked = true)
        {
            IsLocked = isLocked;
        }
    }
}