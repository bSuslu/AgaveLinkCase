using AgaveLinkCase.Chip.Selection;
using AgaveLinkCase.ServiceLocatorSystem;
using AgaveLinkCase.Settings;
using UnityEngine;

namespace AgaveLinkCase.Chip
{
    public class ChipFactory
    {
        private readonly ChipEntity _chipEntityPrefab;
        private readonly IChipConfigSelectionStrategy _chipConfigSelectionStrategy;
        
        public ChipFactory(IChipConfigSelectionStrategy chipConfigSelectionStrategy)
        {
            _chipConfigSelectionStrategy = chipConfigSelectionStrategy;
            _chipEntityPrefab = ServiceLocator.Global.Get<SettingsProvider>().ChipSettings.ChipEntityPrefab;
        }
        public ChipEntity Create(Vector3 position, Transform parent)
        {
            ChipEntity chipEntity = Object.Instantiate(_chipEntityPrefab, position, Quaternion.identity, parent);
            ChipConfig chipConfig = _chipConfigSelectionStrategy.GetChipConfig();
            chipEntity.SetType(chipConfig);
            return chipEntity;
        }
    }
}