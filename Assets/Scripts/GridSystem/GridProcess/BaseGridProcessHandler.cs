using System;
using Cysharp.Threading.Tasks;
using ServiceLocatorSystem;
using Settings;
using UnityEngine;

namespace GridSystem.GridProcess
{
    public abstract class BaseGridProcessHandler : ScriptableObject, IDisposable
    {
        protected GridController _gridController;
        protected VisualSettings _visualSettings;
        protected Grid2D _grid;

        public virtual void Init(GridController gridController)
        {
            _gridController = gridController;
            _grid = _gridController.Grid;
            _visualSettings = ServiceLocator.Global.Get<SettingsProvider>().VisualSettings;
        }

        public abstract UniTask HandleAsync();

        public virtual void Dispose()
        {
        }
    }
}