using UnityEngine;

namespace Scripts.ObjetosScript
{
    public class OnMouse : MonoBehaviour
    {

        [Header("Material")]
        public Material selecionado;
        public Material naoSelecionado;
        private Renderer rend;

        void Start()
        {
            rend = GetComponent<Renderer>();

        }

        private void OnMouseEnter()
        {
            rend.material = selecionado;
        }
        private void OnMouseExit()
        {
            rend.material = naoSelecionado;
        }
    }
}

