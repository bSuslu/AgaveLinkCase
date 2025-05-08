using System;
using System.Collections.Generic;
using AgaveLinkCase.Chip;
using UnityEngine;

namespace AgaveLinkCase.GridSystem
{
    public class LinkController : MonoBehaviour
    {
        [SerializeField] private GridInputSystem _gridInputSystem;

        public event Action<List<Cell>> OnLinkSuccess; 
        public event Action OnLinkReset;
        public event Action<List<Cell>> OnLinkUpdated;
        
        private readonly List<Cell> _linkedCells = new List<Cell>();

        private void Awake()
        {
            _gridInputSystem.OnNewCellTouched += OnNewCellTouched;
            _gridInputSystem.OnRelease += OnRelease;
        }

        private void OnDestroy()
        {
            _gridInputSystem.OnNewCellTouched -= OnNewCellTouched;
            _gridInputSystem.OnRelease -= OnRelease;
        }
        
        private ChipType _activeChipTypeOnLink;
        
        private void OnNewCellTouched(Cell newCell)
        {
           if (!newCell.IsOccupied)
               return;

           if (newCell.IsLocked)
               return;
           
           if (_linkedCells.Count.Equals(0))
           {
               _activeChipTypeOnLink = newCell.ChipEntity.Type;
               LinkCell(newCell);
               return;
           }

           var lastCell = _linkedCells[^1];
           if (!GridUtils.IsAnyNeighbor(newCell.X, newCell.Y, lastCell.X, lastCell.Y))
                return;
           
           if (_activeChipTypeOnLink != newCell.ChipEntity.Type)
               return;

           if (_linkedCells.Count > 1 && newCell == _linkedCells[^2])
           {
               UnlinkLastCell();
               return;
           }
           
           if (_linkedCells.Contains(newCell))
               return;
           
           
           LinkCell(newCell);
        }

        private void UnlinkLastCell()
        {
            _linkedCells.RemoveAt(_linkedCells.Count - 1);
            OnLinkUpdated?.Invoke(_linkedCells);
        }

        private void OnRelease()
        {
            if (IsLinkConditionsMet())
            {
                List<Cell> linkedCells = new List<Cell>(_linkedCells);
                OnLinkSuccess?.Invoke(linkedCells);
            }
            
            _linkedCells.Clear();
            OnLinkReset?.Invoke();
        }

        private bool IsLinkConditionsMet()
        {
            return true;
        }

        private void LinkCell(Cell cell)
        {
            _linkedCells.Add(cell);
            OnLinkUpdated?.Invoke(_linkedCells);
        }
    }
}