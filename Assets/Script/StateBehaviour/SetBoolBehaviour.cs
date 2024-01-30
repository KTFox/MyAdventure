using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolBehaviour : StateMachineBehaviour {

    [SerializeField] private string boolName;
    [SerializeField] private bool updateOnState, updateOnStateMachine;
    [SerializeField] private bool valueOnEnter, valueOnExit;





    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (updateOnState) {
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (updateOnState) {
            animator.SetBool(boolName, valueOnExit);
        }
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash) {
        if (updateOnStateMachine) {
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
        if (updateOnStateMachine) {
            animator.SetBool(boolName, valueOnExit);
        }
    }

}
