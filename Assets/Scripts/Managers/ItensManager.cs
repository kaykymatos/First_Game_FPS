using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Managers
{
    public class ItensManager : MonoBehaviour
    {
        public GameObject[] itens;
        public int index;
        UiManager uiScript;
        float tempo;
        public List<Animator> animItens = new List<Animator>();
        public static ItensManager instance;
        public bool mira;
        private void Awake()
        {
            instance = this;
        }
        void Start()
        {
            index = 0;
            tempo = 0;
            uiScript = GameObject.FindGameObjectWithTag("UiManager").GetComponent<UiManager>();
            for (int i = 0; i < itens.Length; i++)
            {
                animItens.Add(itens[i].GetComponent<Animator>());
            }
        }


        void Update()
        {
            if (!mira)
                MudaArma();
        }
        void MudaArma()
        {
            tempo += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Alpha1) && !animItens[index].GetBool("OcorreAcao"))
            {
                itens[index].SetActive(false);
                index--;
                if (index <= 0)
                {
                    index = itens.Length - 1;
                }
                itens[index].SetActive(true);
                uiScript.iconeImagem.sprite = uiScript.spriteItens[index];
                tempo = 0;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && !animItens[index].GetBool("OcorreAcao"))
            {
                itens[index].SetActive(false);
                index++;
                if (index > itens.Length - 1)
                {
                    index = 0;
                }

                itens[index].SetActive(true);
                uiScript.iconeImagem.sprite = uiScript.spriteItens[index];
                tempo = 0;
            }
            if (itens[index].gameObject.name == "glock" && tempo > 0.05f)
            {
                uiScript.municao.enabled = true;
                uiScript.imagemModoTiro.enabled = true;
            }
            else
            {
                uiScript.municao.enabled = false;
                uiScript.imagemModoTiro.enabled = false;
            }
        }
    }
}