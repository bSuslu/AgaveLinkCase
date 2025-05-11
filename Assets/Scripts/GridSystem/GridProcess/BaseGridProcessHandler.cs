using System;
using AgaveLinkCase.ServiceLocatorSystem;
using AgaveLinkCase.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AgaveLinkCase.GridSystem.GridProcess
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