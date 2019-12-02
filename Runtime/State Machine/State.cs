using System.Collections.Generic;

using UnityEngine;

namespace Tools.StateMachine
{
    public class State : MonoBehaviour
    {
        [Multiline(5)][SerializeField][Tooltip("Editor Only")]
        private string description;

        #region Public Variables
        public List<Transition> transitions = new List<Transition>();

        #endregion

        #region Private Variables

        #endregion

        #region Unity Methods

        /// <summary>
        /// Called when the script is loaded or a value is changed in the
        /// inspector (Called in the editor only).
        /// </summary>
        private void OnValidate()
        {
            foreach (var transition in transitions)
            {
                if (!Application.isPlaying)
                {
                    transition.forceTransition = false;
                }
            }
        }

        public virtual void EnterState()
        {
            gameObject.SetActive(true);
        }

        public virtual void ExitState()
        {
            gameObject.SetActive(false);
        }

        #endregion

        #region Public Methods
        public State CheckTransitions()
        {
            foreach (var transition in transitions)
            {
                var conditionsSucceeded = true;
                foreach (var condition in transition.conditions)
                {
                    if (condition)
                    {
                        conditionsSucceeded = condition.Check();
                    }
                }

                if (conditionsSucceeded && transition.state)
                {
                    return transition.state;
                }

                if (transition.forceTransition && transition.state)
                {
                    transition.forceTransition = false;
                    return transition.state;
                }
            }
            return null;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}