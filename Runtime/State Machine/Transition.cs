using System.Collections.Generic;

using Tools.EditorTools.Attributes;

using UnityEngine;

namespace Tools.StateMachine
{
    [System.Serializable]
    public class Transition
    {
        public enum TransitionType
        {
            True,
            False
        }

        [System.Serializable]
        public class TransitionCondition
        {
            [InspectInline(canEditRemoteTarget = true)]
            public ConditionBase condition;

            public TransitionType returnType;
        }

        [InspectInline(canEditRemoteTarget = true)]
        public State state;

        public List<TransitionCondition> conditions = new List<TransitionCondition>();

        [Header("Debug")][Tooltip("Editor Play Only")]
        public bool forceTransition;
    }
}