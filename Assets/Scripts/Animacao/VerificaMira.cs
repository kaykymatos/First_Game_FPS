using Scripts.Managers;
using UnityEngine;

namespace Scripts.Animacao
{
    public class VerificaMira : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ItensManager.instance.mira = false;
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ItensManager.instance.mira = true;
        }
    }
}