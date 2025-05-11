using AgaveLinkCase.LinkSystem;
using AgaveLinkCase.Pool;
using UnityEngine;

namespace AgaveLinkCase.Chip
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ChipEntity : MonoBehaviour, ILinkable, IPoolable<ChipEntity>
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private IPool<ChipEntity> _chipEntityPool;
        public Vector2Int CellPos { get; set; }
        public Transform Transform => transform;

        public ChipType Type { get; private set; }
        
        public void SetType(ChipConfig chipConfig)
        {
            _spriteRenderer.sprite = chipConfig.Sprite;
            Type = chipConfig.Type;
        }

        public void Reset()
        {
            _chipEntityPool.ReturnToPool(this);
        }
        
        public virtual bool CanBeLinkedWith(ILinkable chip)
        {
            return Type == chip.Type;
        }

        public ChipEntity Get()
        {
            return this;
        }

        public void SetPool(IPool<ChipEntity> pool)
        {
            _chipEntityPool = pool;
        }
    }
}