using System.Collections;
using System.Collections.Generic;

using Tools.EditorTools.Attributes;

using UnityEngine;

namespace Tools.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        #region Public Variables
        [InspectInline(canEditRemoteTarget = true)]
        public State currentState;

        #endregion

        #region Private Variables

        #endregion

        #region Unity Methods

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            currentState?.EnterState();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        public virtual void Update()
        {
            if (currentState)
            {
                CheckTransitions();
            }
        }

        #endregion

        #region Public Methods
        public void CheckTransitions()
        {
            var nextState = currentState.CheckTransitions();

            if (nextState)
            {
                TransitionToState(nextState);
            }
        }

        public void TransitionToState(State nextState)
        {
            currentState?.ExitState();
            currentState = nextState;
            currentState?.EnterState();
        }

        #endregion

        #region Private Methods

        #endregion
    }
}