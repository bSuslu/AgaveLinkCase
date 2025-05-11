using LevelSystem;
using ServiceLocatorSystem;
using Settings;
using UnityEngine;

namespace Services.Initializers
{
    public class GlobalServiceInitializer : MonoBehaviour
    {
        [SerializeField] private SettingsProvider _settingsProvider;

        private void Awake()
        {
            ServiceLocator.Global.Register(_settingsProvider);
            ServiceLocator.Global.Register(new LevelService());
        }
    }
}
