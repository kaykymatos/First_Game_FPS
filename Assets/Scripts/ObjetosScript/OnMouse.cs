using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.ObjetosScript
{

    public class OnMouse : MonoBehaviour
    {
        public Material selecionado, naoSelecionado;
        Renderer rend;

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

