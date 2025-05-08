using UnityEngine;

namespace AgaveLinkCase.Chip
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ChipEntity : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        public ChipType Type { get; private set; }
        
        public void SetType(ChipConfig chipConfig)
        {
            _spriteRenderer.sprite = chipConfig.Sprite;
            Type = chipConfig.Type;
        }

        // TODO Pool
        public void Reset()
        {
            Destroy(gameObject); // TODO Pool();
        }
    }
}