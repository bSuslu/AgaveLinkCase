using Cysharp.Threading.Tasks;
using DG.Tweening;
using EventSystem;
using UnityEngine;

namespace Scene
{
    public class SceneTransitionUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration = 0.5f;

        private EventBinding<SceneTransitionStartedEvent> _startBinding;
        private EventBinding<SceneTransitionCompletedEvent> _completeBinding;

        private void Awake()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(true);

            _startBinding = new EventBinding<SceneTransitionStartedEvent>(OnTransitionStarted);
            _completeBinding = new EventBinding<SceneTransitionCompletedEvent>(OnTransitionCompleted);
            EventBus<SceneTransitionStartedEvent>.Subscribe(_startBinding);
            EventBus<SceneTransitionCompletedEvent>.Subscribe(_completeBinding);
        }

        private void OnDestroy()
        {
            EventBus<SceneTransitionStartedEvent>.Unsubscribe(_startBinding);
            EventBus<SceneTransitionCompletedEvent>.Unsubscribe(_completeBinding);
        }

        private async void OnTransitionStarted(SceneTransitionStartedEvent _)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1f;
        }

        private async void OnTransitionCompleted(SceneTransitionCompletedEvent _)
        {
            await UniTask.Delay(200); 
            await _canvasGroup.DOFade(0f, _fadeDuration).ToUniTask();
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }
    }
}