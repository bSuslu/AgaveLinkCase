using AgaveLinkCase.EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace AgaveLinkCase.Scene
{
    [RequireComponent(typeof(Button))]
    public class SceneTransitionButton : MonoBehaviour
    {
        [SerializeField] private GameScene _scene;
        [SerializeField] private Button _button;
        [SerializeField] private bool _oneFrameLateTransition;
        
        protected void Awake()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        protected void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            EventBus<LoadSceneRequestEvent>.Publish(new LoadSceneRequestEvent(_scene));
        }
    }
}