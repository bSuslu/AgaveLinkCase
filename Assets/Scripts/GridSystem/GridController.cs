using System.Collections.Generic;
using AgaveLinkCase.Chip;
using AgaveLinkCase.Chip.Selection;
using AgaveLinkCase.Helpers;
using AgaveLinkCase.Level;
using AgaveLinkCase.ServiceLocatorSystem;
using AgaveLinkCase.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace AgaveLinkCase.GridSystem
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private GridInputSystem _gridInputSystem;
        [SerializeField] private LinkController _linkController;
        [SerializeField] private CameraHelper _cameraHelper;

        private float _fallOffset = 1.5f;
        private Grid2D _grid;

        private ChipFactory _chipFactory;

        private void Awake()
        {
            _linkController.OnLinkSuccess += OnLinkSuccess;
        }

        private void OnDestroy()
        {
            _linkController.OnLinkSuccess -= OnLinkSuccess;
        }

        private void OnLinkSuccess(List<Cell> cells)
        {
            ProcessGrid(cells).Forget();
        }

        private async UniTaskVoid ProcessGrid(List<Cell> cells)
        {
            HashSet<int> columnIndexLocks = new HashSet<int>();

            foreach (var cell in cells)
            {
                columnIndexLocks.Add(cell.X);
            }

            SetColumnLockState(columnIndexLocks, true);

            await HandleCellClear(cells);
            await HandleFill();
            await HandleFall();

            SetColumnLockState(columnIndexLocks, false);
        }

        private void Start()
        {
            LevelData levelData = ServiceLocator.Global.Get<LevelService>().LevelData;
            GridSettings gridSettings = ServiceLocator.Global.Get<SettingsProvider>().GridSettings;

            _grid = new GridFactory().Create(gridSettings, levelData);

            CreateChips();

            _gridInputSystem.Initialize(_grid);
            _cameraHelper.HandleGridFrustum(_grid.GetWorldPosition(0, 0),
                _grid.GetWorldPosition(_grid.Width, _grid.Height));
        }

        private void CreateChips()
        {
            IChipConfigSelectionStrategy chipConfigSelectionStrategy = new RandomChipConfigSelectionStrategy();
            _chipFactory = new ChipFactory(chipConfigSelectionStrategy);

            for (var x = 0; x < _grid.Width; x++)
            {
                for (var y = 0; y < _grid.Height; y++)
                {
                    Vector3 position = _grid.GetWorldPositionCenter(x, y);
                    ChipEntity chipEntity = _chipFactory.Create(position, transform);
                    _grid.GetCell(x, y).SetChip(chipEntity);
                }
            }
        }

        private async UniTask HandleCellClear(List<Cell> cells)
        {
            foreach (var cell in cells)
            {
                await UniTask.Delay(100);
                cell.DestroyChip();
            }
        }

        private async UniTask HandleFall()
        {
            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    Cell cell = _grid.GetCell(x, y);
                    if (!cell.IsOccupied)
                    {
                        Vector3 position = _grid.GetWorldPositionCenter(x, y);

                        ChipEntity newChipEntity = _chipFactory.Create(position, transform);
                        cell.SetChip(newChipEntity);

                        // Animate falling into place
                        newChipEntity.transform.position =
                            _grid.GetWorldPositionCenter(x, y) + Vector3.up * _fallOffset;
                        newChipEntity.transform.DOMove(_grid.GetWorldPositionCenter(x, y), 0.25f);
                    }
                }
            }
        }

        private async UniTask HandleFill()
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

                        tasks.Add(fallingChipEntity.transform.DOMove(_grid.GetWorldPositionCenter(x, y), .25f)
                            .ToUniTask());

                        break;
                    }
                }
            }


            await UniTask.Delay(1000);
        }

        private void SetColumnLockState(HashSet<int> columns, bool isLocked)
        {
            foreach (var x in columns)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    _grid.GetCell(x, y).IsLocked = isLocked;
                }
            }
        }
    }
}