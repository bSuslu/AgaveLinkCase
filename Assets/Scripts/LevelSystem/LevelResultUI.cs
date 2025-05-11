using AgaveLinkCase.ServiceLocatorSystem;
using UnityEngine;

namespace AgaveLinkCase.LevelSystem
{
    public class LevelResultUI : MonoBehaviour
    {
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;
        private LevelProgressManager _levelProgressManager;

        // TODO changed to start due to race condition - fix it
        private void Start()
        {
            _levelProgressManager = ServiceLocator.ForSceneOf(this).Get<LevelProgressManager>();
            _levelProgressManager.OnLevelFail += OnLevelFail;
            _levelProgressManager.OnLevelSuccess += OnLevelSuccess;
        }
        
        // TODO Dispose to prevent null check
        private void OnDestroy()
        {
            if (_levelProgressManager!= null)
            {
                _levelProgressManager.OnLevelFail -= OnLevelFail;
                _levelProgressManager.OnLevelSuccess -= OnLevelSuccess;
            }
        }

        private void OnLevelSuccess() => _winPanel.SetActive(true);


        private void OnLevelFail() => _losePanel.SetActive(true);
    }
}