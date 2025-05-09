using System.Collections.Generic;
using AgaveLinkCase.Events;
using AgaveLinkCase.EventSystem;
using AgaveLinkCase.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace AgaveLinkCase.GridSystem
{
    public class CellClearHandler : BaseGridProcessHandler
    {
        private Grid2D _grid;
        private VisualSettings _visualSettings;
        private List<Vector2Int> _coords = new List<Vector2Int>();

        public CellClearHandler(Grid2D grid, VisualSettings visualSettings, List<Vector2Int> coords)
        {
            _grid = grid;
            _visualSettings = visualSettings;
            _coords = coords;
        }

        public override async UniTask HandleAsync()
        {
            var cellList = new List<Cell>();
            var taskList = new List<UniTask>();
            foreach (var coord in _coords)
            {
                await UniTask.Delay(_visualSettings.ChipDisappearIntervalMS);
                Cell cell = _grid.GetCell(coord.x, coord.y);
                cellList.Add(cell);
                ;
                taskList.Add(cell.ChipEntity.transform.DOScale(Vector3.zero, _visualSettings.ChipDisappearDuration)
                    .SetEase(_visualSettings.ChipDisappearEase).ToUniTask());
            }

            await UniTask.WhenAll(taskList.ToArray());
            EventBus<LinkCollectedEvent>.Publish(new LinkCollectedEvent(_coords.Count));
            cellList.ForEach(c => c.DestroyChip());
        }
    }
}