using DG.Tweening;
using UnityEngine;

namespace AgaveLinkCase.Settings
{
    [CreateAssetMenu(fileName = "VisualSettings", menuName = "Settings/Visual")]
    public class VisualSettings : ScriptableObject
    {
        [Header("Fill Settings")]
        [SerializeField] private float _fillDuration;
        [SerializeField] private Ease _fillEase;
        public float FillDuration => _fillDuration;
        public Ease FillEase => _fillEase;

        [Header("Fall Settings")]
        [SerializeField] private float _fallDuration;
        [SerializeField] private Ease _fallEase;
        [SerializeField] private float _fallOffset;
        public float FallOffset => _fallOffset;
        public float FallDuration => _fallDuration;
        public Ease FallEase => _fallEase;

        [Header("Chip Disappear Settings")]
        [SerializeField] private float _chipDisappearDuration;
        [SerializeField] private int _chipDisappearIntervalMS;
        [SerializeField] private Ease _chipDisappearEase;
        public float ChipDisappearDuration => _chipDisappearDuration;
        public int ChipDisappearIntervalMS => _chipDisappearIntervalMS;
        public Ease ChipDisappearEase => _chipDisappearEase;

        [Header("Shuffle Settings")]
        [SerializeField] private float _shuffleDuration;
        [SerializeField] private Ease _shuffleEase;
        public float ShuffleDuration => _shuffleDuration;
        public Ease ShuffleEase => _shuffleEase;
    }
}