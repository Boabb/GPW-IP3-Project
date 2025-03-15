using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAudioTrigger : StateMachineBehaviour
{
    [SerializeField] private SoundEffect soundToPlay;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
<<<<<<< Updated upstream:Project/Assets/Scripts/AnimatorAudioTrigger.cs
        AudioManager.PlaySoundEffect(soundToPlay);   
=======
        animator.SetBool("EnterElevator", false);
>>>>>>> Stashed changes:Project/Assets/Scripts/AnimationStateMachine/AnimatorPlayerMovement.cs
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
<<<<<<< Updated upstream:Project/Assets/Scripts/AnimatorAudioTrigger.cs
        AudioManager.StopSoundEffect(soundToPlay);
=======
        GameManager.levelLoader.TransitionToMainMenu();

>>>>>>> Stashed changes:Project/Assets/Scripts/AnimationStateMachine/AnimatorPlayerMovement.cs
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
