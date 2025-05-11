using System.Collections.Generic;
using System.Linq;
using Chip;
using Chip.Selection;
using Cysharp.Threading.Tasks;
using EventSystem;
using GridSystem.GridProcess;
using Helpers;
using LevelSystem;
using LinkSystem;
using ServiceLocatorSystem;
using Settings;
using UnityEngine;

namespace GridSystem
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private GridInputSystem _gridInputSystem;
        [SerializeField] private CameraHelper _cameraHelper;
        [SerializeField] private SpriteRenderer _cellBackgroundSpriteRenderer;
        
        public Grid2D Grid { get; private set; }
        public ChipFactory ChipFactory { get; private set; }
        public List<Vector2Int> LinkedCellsPosition { get; private set; }

        private List<BaseGridProcessHandler> _initialProcessHandlers;
        private List<BaseGridProcessHandler> _linkSuccessProcessHandlers;
        private EventBinding<LinkSuccessEvent> _linkSuccessEventBinding;

        private void Awake()
        {
            _linkSuccessEventBinding = new EventBinding<LinkSuccessEvent>(OnLinkSuccess);
            EventBus<LinkSuccessEvent>.Subscribe(_linkSuccessEventBinding);

            var settingsProvider = ServiceLocator.Global.Get<SettingsProvider>();
            _initialProcessHandlers = settingsProvider.GridSettings.InitialProcessHandlers;
            _linkSuccessProcessHandlers = settingsProvider.GridSettings.LinkSuccessHandlers;
        }

        private void OnDestroy()
        {
            EventBus<LinkSuccessEvent>.Unsubscribe(_linkSuccessEventBinding);
            _initialProcessHandlers.ForEach(x => x.Dispose());
            _linkSuccessProcessHandlers.ForEach(x => x.Dispose());
        }

        private async void Start()
        {
            LevelData levelData = ServiceLocator.Global.Get<LevelService>().LevelData;
            GridSettings gridSettings = ServiceLocator.Global.Get<SettingsProvider>().GridSettings;

            Grid = new GridFactory().Create(gridSettings, levelData);
            ConstructGridBackground();
            CreateChips();
            _gridInputSystem.Initialize(Grid);
            _cameraHelper.HandleGridFrustum(Grid.GetWorldPosition(0, 0),
                Grid.GetWorldPosition(Grid.Width, Grid.Height));

            _initialProcessHandlers.ForEach(x => x.Init(this));
            _linkSuccessProcessHandlers.ForEach(x => x.Init(this));

            await ProcessInitialGrid();
        }

        private void OnLinkSuccess(LinkSuccessEvent linkSuccessEvent)
        {
            ProcessSuccessfulLinkGrid(linkSuccessEvent.Link).Forget();
        }
        
        private async UniTask ProcessInitialGrid()
        {
            var allColumnIndexes = new HashSet<int>(Enumerable.Range(0, Grid.Width));

            SetColumnLockState(allColumnIndexes, true);
            foreach (var handler in _initialProcessHandlers)
            {
                handler.Init(this);
                await handler.HandleAsync();
            }

            SetColumnLockState(allColumnIndexes, false);
        }

        private async UniTaskVoid ProcessSuccessfulLinkGrid(List<ILinkable> cells)
        {
            await UniTask.Yield();
            HashSet<int> columnIndexLocks = new HashSet<int>();
            LinkedCellsPosition = new List<Vector2Int>();

            foreach (var linkable in cells)
            {
                columnIndexLocks.Add(linkable.CellPos.x);
                LinkedCellsPosition.Add(linkable.CellPos);
            }

            SetColumnLockState(columnIndexLocks, true);
            foreach (var handler in _linkSuccessProcessHandlers)
            {
                await handler.HandleAsync();
            }
            SetColumnLockState(columnIndexLocks, false);
        }

        private void ConstructGridBackground()
        {
            var background = Instantiate(_cellBackgroundSpriteRenderer, transform);
            var maxPoint = Grid.GetWorldPosition(Grid.Width, Grid.Height);
            var minPoint = Grid.GetWorldPosition(0, 0);
            background.gameObject.transform.position = (maxPoint - minPoint) / 2f;
            background.size = new Vector2(Grid.Width * Grid.CellSize, Grid.Height * Grid.CellSize);
        }

        // TODO Get from Settings
        public int PoolMultiplierConstant { get; set; } = 2;
        
        private void CreateChips()
        {
            IChipConfigSelectionStrategy chipConfigSelectionStrategy = new RandomChipConfigSelectionStrategy();
            ChipFactory = new ChipFactory();
            ChipFactory.InitPool(Grid.Height * Grid.Width * PoolMultiplierConstant, transform);

            for (var x = 0; x < Grid.Width; x++)
            {
                for (var y = 0; y < Grid.Height; y++)
                {
                    Vector3 position = Grid.GetWorldPositionCenter(x, y);
                    ChipEntity chipEntity = ChipFactory.Create(chipConfigSelectionStrategy);
                    chipEntity.transform.position = position;
                    Grid.GetCell(x, y).SetChip(chipEntity);
                }
            }
        }

        private void SetColumnLockState(HashSet<int> columns, bool isLocked)
        {
            foreach (var x in columns)
            {
                for (int y = 0; y < Grid.Height; y++)
                {
                    Grid.GetCell(x, y).IsLocked = isLocked;
                }
            }
        }
    }
}