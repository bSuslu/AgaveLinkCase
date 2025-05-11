using System.Collections.Generic;
using Chip;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace GridSystem.GridProcess.Fill
{
    [CreateAssetMenu(fileName = "GridFillGridProcessHandler", menuName = "GridProcessHandlers/GridFill")]
    public class GridFillHandler : BaseGridProcessHandler
    {
        [SerializeField] private int _additionalDelay = 1000;
        [SerializeField] private bool _waitFillFinishesBeforeNextProcess = true;

        public override async UniTask HandleAsync()
        {
            await UniTask.Yield();
            
            List<UniTask> tasks = new List<UniTask>();
            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    Cell currentCell = _grid.GetCell(x, y);
                    if (currentCell.IsOccupied)
                        continue;

                    for (int k = y + 1; k < _grid.Height; k++)
                    {
                        Cell upperCell = _grid.GetCell(x, k);
                        if (!upperCell.IsOccupied)
                            continue;


                        ChipEntity fallingChipEntity = upperCell.ChipEntity;
                        upperCell.SetChip(null);
                        currentCell.SetChip(fallingChipEntity);

                        tasks.Add(fallingChipEntity.transform
                            .DOMove(_grid.GetWorldPositionCenter(x, y), _visualSettings.FillDuration)
                            .SetEase(_visualSettings.FillEase).ToUniTask());

                        break;
                    }
                }
            }

            if (_waitFillFinishesBeforeNextProcess)
                await UniTask.WhenAll(tasks.ToArray());

            await UniTask.Delay(_additionalDelay);
            await UniTask.Yield();
        }
    }
}