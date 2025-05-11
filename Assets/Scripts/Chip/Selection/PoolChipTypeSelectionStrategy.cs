using System.Linq;
using ServiceLocatorSystem;
using Settings;

namespace Chip.Selection
{
    public class PoolChipTypeSelectionStrategy : IChipConfigSelectionStrategy
    {
        private ChipType _chipType;
        private readonly ChipConfig[] _chipConfigs;

        public PoolChipTypeSelectionStrategy()
        {
            _chipConfigs = ServiceLocator.Global.Get<SettingsProvider>().ChipSettings.ChipConfigs;
        }

        public void SetType(ChipType chipType)
        {
            _chipType = chipType;
        }

        public ChipConfig GetChipConfig()
        {
            return _chipConfigs.FirstOrDefault(c => c.Type == _chipType);
        }
    }
}