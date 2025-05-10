using AgaveLinkCase.ServiceLocatorSystem;
using TMPro;
using UnityEngine;

namespace AgaveLinkCase.LevelSystem
{
    public class LevelProgressUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _targetScoreText;
        [SerializeField] private TextMeshProUGUI _currentScoreText;
        [SerializeField] private TextMeshProUGUI _moveCountText;
        private LevelProgressManager _levelProgressManager;
        
        // TODO changed to start for race condition - fix it
        private void Start()
        {
            _levelProgressManager = ServiceLocator.ForSceneOf(this).Get<LevelProgressManager>();
            _levelProgressManager.OnMoveCountValueChanged += OnMoveCountValueChanged;
            _levelProgressManager.OnScoreValueChanged += OnScoreValueChanged;

            _targetScoreText.text = _levelProgressManager.TargetScore.ToString();
                
            OnMoveCountValueChanged(_levelProgressManager.MoveCount);
            OnScoreValueChanged(_levelProgressManager.Score);
        }
        private void OnDestroy()
        {
            _levelProgressManager.OnMoveCountValueChanged -= OnMoveCountValueChanged;
            _levelProgressManager.OnScoreValueChanged -= OnScoreValueChanged;   
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