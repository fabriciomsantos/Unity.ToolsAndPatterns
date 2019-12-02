using System.Collections.Generic;

using Tools.EditorTools.Attributes;

using UnityEngine;

namespace Tools.StateMachine
{
    [System.Serializable]
    public class Transition
    {
        [InspectInline(canEditRemoteTarget = true)]
        public State state;

        public List<ConditionBase> conditions = new List<ConditionBase>();

        [Header("Debug")][Tooltip("Editor Play Only")]
        public bool forceTransition;
    }
}