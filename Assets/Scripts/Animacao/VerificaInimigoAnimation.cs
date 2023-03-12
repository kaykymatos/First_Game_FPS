using UnityEngine;

namespace Scripts.Animacao
{
    public class VerificaInimigoAnimation : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("podeAndar", true);
        }
    }
}