using System.Collections.Generic;

using Tools.EditorTools.Attributes;

using UnityEngine;
using UnityEngine.Events;

namespace Tools.StateMachine
{
    public class State : MonoBehaviour
    {
        [Multiline(5)][SerializeField][Tooltip("Editor Only")]
        private string description;

        #region Public Variables
        [Header("Base")]
        public List<Transition> transitions = new List<Transition>();
        public UnityEvent onEnterState = new UnityEvent();
        public UnityEvent onExitState = new UnityEvent();

        [Header("Sub State")][InspectInline(canEditRemoteTarget = true)]
        public State currentSubState;

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
            onEnterState.Invoke();

            currentSubState?.EnterState();
        }

        public virtual void ExitState()
        {
            onExitState.Invoke();
            gameObject.SetActive(false);
        }

        #endregion

        #region Public Methods
        public State CheckTransitions()
        {
            if (currentSubState)
            {
                CheckSubTransitions();
            }

            foreach (var transition in transitions)
            {
                var conditionsSucceeded = true;
                foreach (var condition in transition.conditions)
                {
                    if (condition.condition)
                    {
                        if (condition.returnType == Transition.TransitionType.True && condition.condition.CheckCondition())
                        {
                            conditionsSucceeded = true;
                        }
                        else if (condition.returnType == Transition.TransitionType.False && !condition.condition.CheckCondition())
                        {
                            conditionsSucceeded = true;
                        }
                        else
                        {
                            conditionsSucceeded = false;
                        }
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

        public void CheckSubTransitions()
        {
            var nextState = currentSubState.CheckTransitions();

            if (nextState)
            {
                TransitionToSubState(nextState);
            }
        }

        public void TransitionToSubState(State nextState)
        {
            currentSubState?.ExitState();
            currentSubState = nextState;
            currentSubState?.EnterState();
        }

        #endregion

        #region Private Methods

        #endregion
    }
}