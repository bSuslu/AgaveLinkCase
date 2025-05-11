using System;
using AgaveLinkCase.EventSystem;
using AgaveLinkCase.LevelSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AgaveLinkCase.GridSystem
{
    // TODO: new Input system
    public class GridInputSystem : MonoBehaviour, InputSystem.IPlayerActions
    {
        [SerializeField] private Camera _camera;
        
        public event Action<Cell> OnNewCellTouched;
        public event Action OnRelease;
        
        private Cell _activeCell;
        private Grid2D _grid2D;

        private bool _canRead = false;
        private EventBinding<LevelCompletedEvent> _levelCompletedEventBinding;
        
        private InputSystem _inputSystem;

        private void Awake()
        {
            _levelCompletedEventBinding = new EventBinding<LevelCompletedEvent>(OnLevelCompletedEvent);
            EventBus<LevelCompletedEvent>.Subscribe(_levelCompletedEventBinding);
            
            _inputSystem = new InputSystem();
            _inputSystem.Player.SetCallbacks(this);
            _inputSystem.Player.Enable();
            _inputSystem.Enable();
        }

        private void OnDestroy()
        {
            EventBus<LevelCompletedEvent>.Unsubscribe(_levelCompletedEventBinding);
            
            _inputSystem.Player.SetCallbacks(null);
            _inputSystem.Player.Disable();
            _inputSystem.Disable();
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

        private bool _isTouching;
        public void OnPress(InputAction.CallbackContext context)
        {
            _isTouching = !context.canceled;
            
            if (context.canceled)
            {
                _activeCell = null;
                OnRelease?.Invoke();
            }
        }

        public void OnPosition(InputAction.CallbackContext context)
        {
            if (_isTouching && _canRead)
            {
                CheckIfActiveCellChanged(context.ReadValue<Vector2>());
            }
        }
        
        private void CheckIfActiveCellChanged(Vector2 mousePosition)
        {
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