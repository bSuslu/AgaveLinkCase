using System.Collections.Generic;
using AgaveLinkCase.Chip;
using AgaveLinkCase.Chip.Selection;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace AgaveLinkCase.GridSystem.GridProcess.Fall
{
    [CreateAssetMenu(fileName = "GridFallGridProcessHandler", menuName = "GridProcessHandlers/GridFall")]
    public class GridFallHandler : BaseGridProcessHandler
    {
        private IChipConfigSelectionStrategy _chipConfigSelectionStrategy;

        public override void Init(GridController gridController)
        {
            base.Init(gridController);
            _chipConfigSelectionStrategy = new RandomChipConfigSelectionStrategy();
        }

        public override async UniTask HandleAsync()
        {
            await UniTask.Yield();

            var taskList = new List<UniTask>();
            for (int x = 0; x < _gridController.Grid.Width; x++)
            {
                for (int y = 0; y < _gridController.Grid.Height; y++)
                {
                    Cell cell = _gridController.Grid.GetCell(x, y);
                    if (!cell.IsOccupied)
                    {
                        Vector3 position = _gridController.Grid.GetWorldPositionCenter(x, y);

                        ChipEntity newChipEntity =
                            _gridController.ChipFactory.Create(_chipConfigSelectionStrategy);
                        cell.SetChip(newChipEntity);

                        // Animate falling into place
                        newChipEntity.transform.position =
                            _gridController.Grid.GetWorldPositionCenter(x, y) + Vector3.up * _visualSettings.FallOffset;

                        taskList.Add(newChipEntity.transform
                            .DOMove(_gridController.Grid.GetWorldPositionCenter(x, y), _visualSettings.FallDuration)
                            .SetEase(_visualSettings.FallEase).ToUniTask());
                    }
                }
            }

            await UniTask.WhenAll(taskList.ToArray());

            await UniTask.Yield();
        }
    }
}