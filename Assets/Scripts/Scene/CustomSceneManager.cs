using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class CustomSceneManager : MonoBehaviour
    {
        [SerializeField] private GameScene _firstSceneToLoad;
        
        private GameScene _currentScene;
        private bool _isLoading;
        private readonly HashSet<string> _cachedSceneNames = new();
        
        private EventBinding<LoadSceneRequestEvent> _loadSceneRequestEventBinding;
        
        private void Awake()
        {
            CacheBuildScenes();
            
            _loadSceneRequestEventBinding = new EventBinding<LoadSceneRequestEvent>(OnLoadSceneRequest);
            EventBus<LoadSceneRequestEvent>.Subscribe(_loadSceneRequestEventBinding);
        }
        
        private void OnDestroy()
        {
            EventBus<LoadSceneRequestEvent>.Unsubscribe(_loadSceneRequestEventBinding);
        }

        private void OnLoadSceneRequest(LoadSceneRequestEvent loadSceneRequestEvent)
        {
            LoadScene(loadSceneRequestEvent.Scene);
        }

        private async void Start()
        {
            await LoadFirstScene();
        }

        private async UniTask LoadFirstScene()
        {
            await LoadSceneAsync(_firstSceneToLoad);
        }

        private void LoadScene(GameScene sceneKey)
        {
            if (_isLoading || _currentScene == sceneKey)
            {
                Debug.LogWarning($"Scene '{GetSceneName(sceneKey)}' is already loading or loaded.");
                return;
            }
            
            EventBus<OnBeforeSceneUnloadEvent>.Publish(new OnBeforeSceneUnloadEvent());

            LoadSceneAsync(sceneKey).Forget();
        }

        private async UniTask LoadSceneAsync(GameScene sceneKey)
        {
            _isLoading = true;

            EventBus<SceneTransitionStartedEvent>.Publish(new SceneTransitionStartedEvent());

            string sceneName = GetSceneName(sceneKey);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (asyncLoad == null)
            {
                Debug.LogError($"[Scene Error] Failed to load scene '{sceneName}'.");
                _isLoading = false;

                EventBus<SceneTransitionCompletedEvent>.Publish(new SceneTransitionCompletedEvent());
                return;
            }

            await asyncLoad.ToUniTask();

            _currentScene = sceneKey;
            CleanupScenes(sceneKey);

            EventBus<SceneTransitionCompletedEvent>.Publish(new SceneTransitionCompletedEvent());
            _isLoading = false;
        }

        private void CleanupScenes(GameScene loadedSceneKey)
        {
            List<string> scenesToUnload = new List<string>();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                UnityEngine.SceneManagement.Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name != GetSceneName(GameScene.PersistentScene) && scene.name != GetSceneName(loadedSceneKey))
                {
                    scenesToUnload.Add(scene.name);
                }
            }

            foreach (var sceneName in scenesToUnload)
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        private string GetSceneName(GameScene scene)
        {
            string sceneName = scene.ToString();

            if (!IsSceneInBuildSettings(sceneName))
            {
                Debug.LogError(
                    $"[Scene Error] The scene '{sceneName}' mapped from enum '{scene}' is not added to Build Settings or the name is incorrect.");
            }

            return sceneName;
        }

        private void CacheBuildScenes()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < sceneCount; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
                _cachedSceneNames.Add(sceneName);
            }
        }

        private bool IsSceneInBuildSettings(string sceneName) => _cachedSceneNames.Contains(sceneName);
    }
}