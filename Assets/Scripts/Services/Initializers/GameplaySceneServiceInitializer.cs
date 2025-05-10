using System;
using System.Collections.Generic;
using AgaveLinkCase.LevelSystem;
using AgaveLinkCase.ServiceLocatorSystem;
using UnityEngine;

namespace AgaveLinkCase.Services.Initializers
{
    public class GameplaySceneServiceInitializer : MonoBehaviour
    {
        private List<IDisposable> _disposableServices = new List<IDisposable>();
        private void Awake()
        {
            var levelService = ServiceLocator.Global.Get<LevelService>();
            var levelProgressManager = new LevelProgressManager(levelService.LevelData.TargetScore,levelService.LevelData.MoveCount);
            _disposableServices.Add(levelProgressManager);
            ServiceLocator.ForSceneOf(this).Register(levelProgressManager);
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _disposableServices.Count; i++)
            {
                _disposableServices[i].Dispose();
            }
        }
    }
}