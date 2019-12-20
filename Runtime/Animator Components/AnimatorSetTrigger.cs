using UnityEngine;

namespace Tools.AnimatorBehaviour
{
    public class AnimatorSetTrigger : StateMachineBehaviour
    {
        public string parameter;
        public AnimatorState state;

        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (state == AnimatorState.OnStateEnter && parameter.Length > 0)
            {
                animator.SetTrigger(parameter);
            }
        }

        // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (state == AnimatorState.OnStateUpdate && parameter.Length > 0)
            {
               animator.SetTrigger(parameter);
            }
        }

        // OnStateExit is called before OnStateExit is called on any state inside this state machine
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (state == AnimatorState.OnStateExit && parameter.Length > 0)
            {
               animator.SetTrigger(parameter);
            }
        }

        // OnStateMove is called before OnStateMove is called on any state inside this state machine
        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (state == AnimatorState.OnStateMove && parameter.Length > 0)
            {
                animator.SetTrigger(parameter);
            }
        }

        // OnStateIK is called before OnStateIK is called on any state inside this state machine
        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (state == AnimatorState.OnStateIK && parameter.Length > 0)
            {
                animator.SetTrigger(parameter);
            }
        }

        // OnStateMachineEnter is called when entering a state machine via its Entry Node
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            if (state == AnimatorState.OnStateMachineEnter && parameter.Length > 0)
            {
                animator.SetTrigger(parameter);
            }
        }

        // OnStateMachineExit is called when exiting a state machine via its Exit Node
        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            if (state == AnimatorState.OnStateMachineExit && parameter.Length > 0)
            {
                animator.SetTrigger(parameter);
            }
        }
    }
}