using UnityEngine;

namespace Scripts.Animacao
{


    public class VerificaAcao : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("OcorreAcao", false);
        }
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("OcorreAcao", true);

        }
    }
}