using UnityEngine;

public class Set8AttackBehaviour : StateMachineBehaviour
{
    private Set8Movement movement;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        CheckPlayerMovement(animator);
        
        if(movement != null) {
            //Debug.Log("setting attack!");
            movement.isAttacking = true;
        }                    
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        CheckPlayerMovement(animator);
        
        if (movement != null) {
            //Debug.Log("resetting attack!");
            movement.isAttacking = false;
            movement.Retract();
        }            
    }

    private void CheckPlayerMovement(Animator animator) {

        if (movement != null) return;

        //playerMovement = FindObjectOfType<PlayerMovement>();
        movement = animator.GetComponent<Set8Movement>();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
