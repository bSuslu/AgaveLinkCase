using System.Collections.Generic;
using AgaveLinkCase.Chip;
using AgaveLinkCase.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace AgaveLinkCase.GridSystem
{
    public class GridFallHandler : BaseGridProcessHandler
    {
        private Grid2D _grid;
        private VisualSettings _visualSettings;
        private ChipFactory _chipFactory;
        private Transform _parenTransform;

        public GridFallHandler(Grid2D grid, VisualSettings visualSettings, ChipFactory chipFactory, Transform parenTransform)
        {
            _grid = grid;
            _visualSettings = visualSettings;
            _chipFactory = chipFactory;
            _parenTransform = parenTransform;
        }

        public override async UniTask HandleAsync()
        {
            var taskList = new List<UniTask>();
            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    Cell cell = _grid.GetCell(x, y);
                    if (!cell.IsOccupied)
                    {
                        Vector3 position = _grid.GetWorldPositionCenter(x, y);

                        ChipEntity newChipEntity = _chipFactory.Create(position, _parenTransform);
                        cell.SetChip(newChipEntity);

                        // Animate falling into place
                        newChipEntity.transform.position =
                            _grid.GetWorldPositionCenter(x, y) + Vector3.up * _visualSettings.FallOffset;
                        
                        taskList.Add(newChipEntity.transform
                            .DOMove(_grid.GetWorldPositionCenter(x, y), _visualSettings.FallDuration)
                            .SetEase(_visualSettings.FallEase).ToUniTask());
                    }
                }
            }

            await UniTask.WhenAll(taskList.ToArray());
        }
    }
}