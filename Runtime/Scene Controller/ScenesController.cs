using System.Collections.Generic;

using Tools.GamePatterns;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Tools.Scene
{
    public class ScenesController : Singleton<ScenesController>
    {
        [System.Serializable]
        public class LevelLoadedEvent : UnityEvent<int>
        { }

        #region Public Variables
        public bool LoadScenesOnEnable;
        [Header("Scenes")]
        public List<SceneReference> baseScenes = new List<SceneReference>();
        public List<SceneReference> levelScenes = new List<SceneReference>();

        [Min(0)]
        public int currentLevel;

        [Header("Events")]
        public UnityEvent OnBaseScenesLoaded = new UnityEvent();

        public LevelLoadedEvent OnLevelLoaded = new LevelLoadedEvent();
        #endregion

        #region Private Variables

        #endregion

        #region Unity Methods

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            if (LoadScenesOnEnable)
            {
                foreach (var scene in baseScenes)
                {
                    LoadSceneAsync(scene, LoadSceneMode.Additive);
                }

                OnBaseScenesLoaded.Invoke();
            }
        }
        #endregion

        #region Public Methods
        public void LoadScene(SceneReference scene, LoadSceneMode loadSceneMode)
        {
            if (!SceneManager.GetSceneByName(scene).isLoaded)
            {
                SceneManager.LoadScene(scene, loadSceneMode);
            }
        }

        public AsyncOperation LoadSceneAsync(SceneReference scene, LoadSceneMode loadSceneMode)
        {
            AsyncOperation operation = null;
            if (!SceneManager.GetSceneByName(scene).isLoaded)
            {
                operation = SceneManager.LoadSceneAsync(scene, loadSceneMode);
            }
            return operation;
        }

        public void UnloadScene(SceneReference scene)
        {
            if (SceneManager.GetSceneByName(scene).isLoaded)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        public void LoadLevel(int index)
        {
            if (index >= levelScenes.Count)
            {
                Debug.LogError("index out of range");
                return;
            }
            UnloadScene(levelScenes[currentLevel]);
            currentLevel = index;
            LoadScene(levelScenes[index], LoadSceneMode.Additive);

            OnLevelLoaded.Invoke(currentLevel);
        }

        public AsyncOperation LoadLevelAsync(int index)
        {
            if (index >= levelScenes.Count)
            {
                Debug.LogError("index out of range");
                return null;
            }
            UnloadScene(levelScenes[currentLevel]);
            currentLevel = index;

            var asyncOperation = LoadSceneAsync(levelScenes[index], LoadSceneMode.Additive);

            OnLevelLoaded.Invoke(currentLevel);

            return asyncOperation;
        }

        public AsyncOperation LoadLevelAsync(SceneReference scene)
        {
            if (!levelScenes.Contains(scene))
            {
                Debug.LogError("scene it not a level");
                return null;
            }
            UnloadScene(levelScenes[currentLevel]);
            currentLevel = levelScenes.IndexOf(scene);

            var asyncOperation = LoadSceneAsync(scene, LoadSceneMode.Additive);

            OnLevelLoaded.Invoke(currentLevel);

            return asyncOperation;
        }

        #endregion

        #region Private Methods

        #endregion

    }
}