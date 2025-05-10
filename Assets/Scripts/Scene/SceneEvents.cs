using AgaveLinkCase.EventSystem;

namespace AgaveLinkCase.Scene
{
    public struct LoadSceneRequestEvent : IEvent
    {
        public GameScene Scene { get; set; }
        
        public LoadSceneRequestEvent(GameScene scene)
        {
            Scene = scene;
        }
    }
    
    public struct SceneTransitionStartedEvent : IEvent
    {
    }

    public struct SceneTransitionCompletedEvent : IEvent
    {
    }
    
    public struct OnBeforeSceneUnloadEvent : IEvent
    {
        
    }
}