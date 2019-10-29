using Tools.GamePatterns;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tools.Components.Scene
{
    public class ScenesController : Singleton<ScenesController>
    {
        #region Public Variables
        public bool LoadScenesOnEnable;
        public SceneReference[] baseScenes;
        public SceneReference[] levelsScenes;
        #endregion

        #region Private Variables

        #endregion

        #region Unity Methods

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            if (LoadScenesOnEnable)
            {
                foreach (var scene in baseScenes)
                {
                    LoadSceneAsync(scene, LoadSceneMode.Additive);
                }
            }
        }
        #endregion

        #region Public Methods
        public static void LoadScene(SceneReference scene, LoadSceneMode loadSceneMode)
        {
            if (!SceneManager.GetSceneByName(scene).isLoaded)
            {
                SceneManager.LoadScene(scene, loadSceneMode);
            }
        }
    
        public static AsyncOperation LoadSceneAsync(SceneReference scene, LoadSceneMode loadSceneMode)
        {
            AsyncOperation operation = null;
            if (!SceneManager.GetSceneByName(scene).isLoaded)
            {
                operation = SceneManager.LoadSceneAsync(scene, loadSceneMode);
            }
            return operation;
        }
    
        public static void UnloadScene(SceneReference scene)
        {
            if (SceneManager.GetSceneByName(scene).isLoaded)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        #endregion

        #region Private Methods

        #endregion

    }
}