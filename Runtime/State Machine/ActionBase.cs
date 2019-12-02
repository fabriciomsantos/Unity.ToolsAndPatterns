using UnityEngine;

namespace Tools.StateMachine
{
    public abstract class ActionBase : MonoBehaviour
    {
        [Multiline(5)][SerializeField][Tooltip("Editor Only")]
        private string description;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        public abstract void OnEnable();

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        public abstract void OnDisable();
    }
}