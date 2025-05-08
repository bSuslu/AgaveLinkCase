using AgaveLinkCase.ServiceLocatorSystem;
using AgaveLinkCase.Settings;
using UnityEngine;

namespace AgaveLinkCase.Chip.Selection
{
    public class RandomChipConfigSelectionStrategy : IChipConfigSelectionStrategy
    {
        private readonly ChipConfig[] _chipConfigs;

        public RandomChipConfigSelectionStrategy()
        {
            _chipConfigs = ServiceLocator.Global.Get<SettingsProvider>().ChipSettings.ChipConfigs;
        }
        public ChipConfig GetChipConfig()
        {
            return _chipConfigs[Random.Range(0, _chipConfigs.Length)];
        }
    }
}