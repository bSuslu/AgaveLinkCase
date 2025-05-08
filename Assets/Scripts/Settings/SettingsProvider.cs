using UnityEngine;

namespace AgaveLinkCase.Settings
{
    public class SettingsProvider : MonoBehaviour
    {
        
        [field: SerializeField] public ChipSettings ChipSettings { get; private set; }

    }
}