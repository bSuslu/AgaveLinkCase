using ServiceLocatorSystem;
using TMPro;
using UnityEngine;

namespace LevelSystem
{
    public class LevelProgressUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _targetScoreText;
        [SerializeField] private TextMeshProUGUI _currentScoreText;
        [SerializeField] private TextMeshProUGUI _moveCountText;
        private LevelProgressManager _levelProgressManager;
        
        private void Start()
        {
            _levelProgressManager = ServiceLocator.ForSceneOf(this).Get<LevelProgressManager>();
            _levelProgressManager.OnMoveCountValueChanged += OnMoveCountValueChanged;
            _levelProgressManager.OnScoreValueChanged += OnScoreValueChanged;

            _targetScoreText.text = _levelProgressManager.TargetScore.ToString();
                
            OnMoveCountValueChanged(_levelProgressManager.MoveCount);
            OnScoreValueChanged(_levelProgressManager.Score);
        }
        
        // TODO Dispose to prevent null check
        private void OnDestroy()
        {
            if (_levelProgressManager!= null)
            {
                _levelProgressManager.OnMoveCountValueChanged -= OnMoveCountValueChanged;
                _levelProgressManager.OnScoreValueChanged -= OnScoreValueChanged;  
            }
        }

        private void OnScoreValueChanged(int value)
        {
            _currentScoreText.text = value.ToString();
        }
        private void OnMoveCountValueChanged(int value)
        {
            _moveCountText.text = value.ToString();
        }
    }
}