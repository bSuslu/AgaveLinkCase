using Chip;
using UnityEngine;

namespace LinkSystem
{
    public interface ILinkable
    {
        public bool CanBeLinkedWith(ILinkable chip);
        public ChipType Type { get; }
        public Vector2Int CellPos { get; }
        public Transform Transform { get; }
    }
}