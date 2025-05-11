using UnityEngine;

namespace Chip
{
    [CreateAssetMenu(fileName = "ChipConfig", menuName = "Config/Chip")]
    public class ChipConfig : ScriptableObject
    {
        [field: SerializeField] public ChipType Type { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}