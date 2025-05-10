using System.Collections.Generic;
using AgaveLinkCase.Chip;
using AgaveLinkCase.Chip.Selection;
using AgaveLinkCase.Helpers;
using AgaveLinkCase.LevelSystem;
using AgaveLinkCase.LinkSystem;
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
        [SerializeField] private SpriteRenderer _cellBackgroundSpriteRenderer;
        
        private Grid2D _grid;
        private ChipFactory _chipFactory;

        private VisualSettings _visualSettings;

        private void Awake()
        {
            _linkController.OnLinkSuccess += OnLinkSuccess;

            _visualSettings = ServiceLocator.Global.Get<SettingsProvider>().VisualSettings;
        }

        private void OnDestroy()
        {
            _linkController.OnLinkSuccess -= OnLinkSuccess;
        }

        private void Start()
        {
            LevelData levelData = ServiceLocator.Global.Get<LevelService>().LevelData;
            GridSettings gridSettings = ServiceLocator.Global.Get<SettingsProvider>().GridSettings;

            _grid = new GridFactory().Create(gridSettings, levelData);
            ConstructGridBackground();
            CreateChips();

            _gridInputSystem.Initialize(_grid);
            _cameraHelper.HandleGridFrustum(_grid.GetWorldPosition(0, 0),
                _grid.GetWorldPosition(_grid.Width, _grid.Height));
        }
        
        private void OnLinkSuccess(List<ILinkable> cells)
        {
            ProcessGrid(cells).Forget();
        }

        private async UniTaskVoid ProcessGrid(List<ILinkable> linkables) //TODO LinkData class
        {
            HashSet<int> columnIndexLocks = new HashSet<int>();
            List<Vector2Int> coords = new List<Vector2Int>();

            foreach (var linkable in linkables)
            {
                columnIndexLocks.Add(linkable.CellPos.x);
                coords.Add(linkable.CellPos);
            }

            SetColumnLockState(columnIndexLocks, true);

            var cellClearHandler = new CellClearHandler(_grid, _visualSettings, coords);
            var fillHandler = new GridFillHandler(_grid, _visualSettings);
            var fallHandler = new GridFallHandler(_grid, _visualSettings, _chipFactory, this.transform);
            var shuffleHandler = new ShuffleHandler(_grid, _visualSettings, coords);

            var handlerList = new List<BaseGridProcessHandler>();
            handlerList.Add(cellClearHandler);
            handlerList.Add(fillHandler);
            handlerList.Add(fallHandler);
            handlerList.Add(shuffleHandler);
            
            foreach (var handler in handlerList)
            {
                await handler.HandleAsync();
            }

            SetColumnLockState(columnIndexLocks, false);
        }
        
        // TODO: Dependent to grid settings, fix it
        private void ConstructGridBackground()
        {
            var background = Instantiate(_cellBackgroundSpriteRenderer, transform);
            var maxPoint = _grid.GetWorldPosition(_grid.Width, _grid.Height);
            var minPoint = _grid.GetWorldPosition(0, 0);
            background.gameObject.transform.position = (maxPoint - minPoint) / 2f;
            background.size = new Vector2(_grid.Width * _grid.CellSize, _grid.Height * _grid.CellSize);
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