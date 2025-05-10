using AgaveLinkCase.EventSystem;
using AgaveLinkCase.Scene;

namespace AgaveLinkCase.Events
{
    public struct LoadSceneRequestEvent : IEvent
    {
        public GameScene Scene { get; set; }
        
        public LoadSceneRequestEvent(GameScene scene)
        {
            Scene = scene;
        }
    }
}