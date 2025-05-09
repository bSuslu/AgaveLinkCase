using System.Collections.Generic;
using AgaveLinkCase.Chip;
using AgaveLinkCase.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace AgaveLinkCase.GridSystem
{
    public class GridFillHandler : BaseGridProcessHandler
    {
        private Grid2D _grid;
        private VisualSettings _visualSettings;

        public GridFillHandler(Grid2D grid, VisualSettings visualSettings)
        {
            _grid = grid;
            _visualSettings = visualSettings;
        }

        public override async UniTask HandleAsync()
        {
            List<UniTask> tasks = new List<UniTask>();
            HashSet<int> columnIndexLocks = new HashSet<int>();

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

                        columnIndexLocks.Add(x);

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

            await UniTask.Delay(100);
        }
    }
}