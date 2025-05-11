using System.Collections.Generic;
using AgaveLinkCase.Chip.Selection;
using AgaveLinkCase.Pool;
using AgaveLinkCase.ServiceLocatorSystem;
using AgaveLinkCase.Settings;
using UnityEngine;

namespace AgaveLinkCase.Chip
{
    public class ChipFactory : IPool<ChipEntity>
    {
        private readonly ChipEntity _chipEntityPrefab;
        private readonly Queue<ChipEntity> _chipEntityPool = new();

        public ChipFactory()
        {
            _chipEntityPrefab = ServiceLocator.Global.Get<SettingsProvider>().ChipSettings.ChipEntityPrefab;
        }

        public void InitPool(int poolSize)
        {
            var parent = new GameObject("ChipPool").transform;

            for (int i = 0; i < poolSize; i++)
            {
                ChipEntity chipEntityInstance = Object.Instantiate(_chipEntityPrefab, parent);
                chipEntityInstance.gameObject.SetActive(false);
                _chipEntityPool.Enqueue(chipEntityInstance);
                chipEntityInstance.SetPool(this);
            }
        }

        public ChipEntity Create(IChipConfigSelectionStrategy chipConfigSelectionStrategy)
        {
            ChipEntity chipEntity = _chipEntityPool.Dequeue();
            chipEntity.gameObject.SetActive(true);
            ChipConfig chipConfig = chipConfigSelectionStrategy.GetChipConfig();
            chipEntity.SetType(chipConfig);
            return chipEntity;
        }


        public void ReturnToPool(IPoolable<ChipEntity> poolable)
        {
            _chipEntityPool.Enqueue(poolable.Get());
            poolable.Get().gameObject.SetActive(false);
        }
    }
}