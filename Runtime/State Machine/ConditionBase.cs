using UnityEngine;

namespace Tools.StateMachine
{
    public abstract class ConditionBase : MonoBehaviour
    {
        [Multiline(5)][SerializeField]
        private string description;

        public abstract bool Check();
    }
}