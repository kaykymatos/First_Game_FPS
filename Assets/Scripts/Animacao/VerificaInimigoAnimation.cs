using UnityEngine;

namespace Scripts.Animacao
{
    public class VerificaInimigoAnimation : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("podeAndar", true);
        }

    }
}