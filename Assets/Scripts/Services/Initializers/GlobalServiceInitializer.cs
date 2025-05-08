using AgaveLinkCase.ServiceLocatorSystem;
using AgaveLinkCase.Settings;
using UnityEngine;

namespace AgaveLinkCase.Services.Initializers
{
    public class GlobalServiceInitializer : MonoBehaviour
    {
        [SerializeField] private SettingsProvider _settingsProvider;

        private void Awake()
        {
            ServiceLocator.Global.Register(_settingsProvider);
        }
    }
}
