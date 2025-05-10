using AgaveLinkCase.LinkSystem;
using UnityEngine;

namespace AgaveLinkCase.Chip
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ChipEntity : MonoBehaviour, ILinkable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        public Vector2Int CellPos { get; set; }
        public Transform Transform => transform;

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
        
        public virtual bool CanBeLinkedWith(ILinkable chip)
        {
            return Type == chip.Type;
        }
    }
}