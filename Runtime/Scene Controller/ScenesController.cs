using Tools.GamePatterns;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tools.Scene
{
    public class ScenesController : Singleton<ScenesController>
    {
        #region Public Variables
        public bool LoadScenesOnEnable;
        public SceneReference[] baseScenes;
        public SceneReference[] levelScenes;

        [Min(0)]
        public int currentLevel;
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
                LoadSceneAsync(levelScenes[currentLevel], LoadSceneMode.Additive);
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
            UnloadScene(levelScenes[currentLevel]);
            currentLevel = index;
            LoadScene(levelScenes[index], LoadSceneMode.Additive);
        }

        public AsyncOperation LoadLevelAsync(int index)
        {
            UnloadScene(levelScenes[currentLevel]);
            currentLevel = index;
            return LoadSceneAsync(levelScenes[index], LoadSceneMode.Additive);
        }

        #endregion

        #region Private Methods

        #endregion

    }
}