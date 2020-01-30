using UnityEngine;

namespace Tools.StateMachine
{
    public abstract class ConditionBase : MonoBehaviour
    {
        [Multiline(5)][SerializeField]
        private string description;
        [SerializeField][Tooltip("Read Only")]
        private bool returnedValue;

        public bool CheckCondition()
        {
            returnedValue = Check();
            return returnedValue;
        }

        public abstract bool Check();
    }
}