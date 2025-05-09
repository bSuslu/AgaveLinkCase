using System;
using AgaveLinkCase.Events;
using AgaveLinkCase.EventSystem;
using UnityEngine;

namespace AgaveLinkCase.GridSystem
{
    // TODO: new Input system
    public class GridInputSystem : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        public event Action<Cell> OnNewCellTouched;
        public event Action OnRelease;
        
        private Cell _activeCell;
        private Grid2D _grid2D;

        private bool _canRead = false;
        private EventBinding<LevelCompletedEvent> _levelCompletedEventBinding;

        private void Awake()
        {
            _levelCompletedEventBinding = new EventBinding<LevelCompletedEvent>(OnLevelCompletedEvent);
            EventBus<LevelCompletedEvent>.Subscribe(_levelCompletedEventBinding);
        }

        private void OnDestroy()
        {
            EventBus<LevelCompletedEvent>.Unsubscribe(_levelCompletedEventBinding);
        }

        private void OnLevelCompletedEvent(LevelCompletedEvent obj)
        {
            _canRead = false;
        }

        public void Initialize(Grid2D grid2D)
        {
            _grid2D = grid2D;
            _canRead = true;
        }
        
        private void Update()
        {
            if (!_canRead)
                return;
            
            if (Input.GetMouseButton(0))
            {
                CheckIfActiveCellChanged();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _activeCell = null;
                OnRelease?.Invoke();
            }
        }
        
        private void CheckIfActiveCellChanged()
        {
            var mousePosition = Input.mousePosition;
            var worldPosition = _camera.ScreenToWorldPoint(mousePosition);

            if (_grid2D.TryGetCell(worldPosition, out Cell cell))
            {
                if (cell != _activeCell)
                {
                    _activeCell = cell;
                    OnNewCellTouched?.Invoke(cell);
                }
            }
        }
    }
}