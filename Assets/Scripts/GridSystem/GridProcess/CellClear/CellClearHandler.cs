using System.Collections.Generic;
using System.Linq;
using AgaveLinkCase.EventSystem;
using AgaveLinkCase.LinkSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace AgaveLinkCase.GridSystem.GridProcess.CellClear
{
    [CreateAssetMenu(fileName = "CellClearGridProcessHandler", menuName = "GridProcessHandlers/CellClear")]
    public class CellClearHandler : BaseGridProcessHandler
    {
        private List<Vector2Int> _cellsPositions;
        private EventBinding<LinkSuccessEvent> _linkSuccessEventBinding;
        public override void Init(GridController gridController)
        {
            base.Init(gridController);
            _cellsPositions = _gridController.LinkedCellsPosition;
            
            _linkSuccessEventBinding = new EventBinding<LinkSuccessEvent>(OnLinkSuccess);
            EventBus<LinkSuccessEvent>.Subscribe(_linkSuccessEventBinding);
        }

        private void OnLinkSuccess(LinkSuccessEvent linkSuccessEvent)
        {
            _cellsPositions = linkSuccessEvent.Link.ConvertAll(c => c.CellPos).ToList();
        }

        public override async UniTask HandleAsync()
        {
            await UniTask.Yield();
            var cellList = new List<Cell>();
            var taskList = new List<UniTask>();
            foreach (var coord in _cellsPositions)
            {
                await UniTask.Delay(_visualSettings.ChipDisappearIntervalMS);
                Cell cell = _grid.GetCell(coord.x, coord.y);
                cellList.Add(cell);
                taskList.Add(cell.ChipEntity.transform.DOScale(Vector3.zero, _visualSettings.ChipDisappearDuration)
                    .SetEase(_visualSettings.ChipDisappearEase).ToUniTask());
            }

            await UniTask.WhenAll(taskList.ToArray());
            
            EventBus<LinkCollectedEvent>.Publish(new LinkCollectedEvent(_cellsPositions.Count));
            
            cellList.ForEach(c => c.DestroyChip());
            
            await UniTask.Yield();
        }

        public override void Dispose()
        {
            base.Dispose();
            EventBus<LinkSuccessEvent>.Unsubscribe(_linkSuccessEventBinding);
        }
    }
}