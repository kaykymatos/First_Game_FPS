using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Raycast
{
    public class RaycastScript : MonoBehaviour
    {
        public float distanciaAlvo;
        public GameObject objArrastar, objPegar;
        RaycastHit hit;
        public Text textBotao, textInfo;

        void Update()
        {
            if (Time.frameCount % 5 == 0)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5, Color.red);
                bool verificaFisica = Physics.SphereCast(transform.position, 0.1f, transform.TransformDirection(Vector3.forward), out hit);
                if (verificaFisica)
                {
                    distanciaAlvo = hit.distance;
                    if (hit.transform.gameObject.CompareTag("objArrasta"))
                    {
                        objArrastar = hit.transform.gameObject;
                        objPegar = null;

                        textBotao.text = "[E]";
                        textInfo.text = "Arrastar/Soltar";
                    }else
                    if (hit.transform.gameObject.CompareTag("objPegar"))
                    {
                        objPegar = hit.transform.gameObject;
                        objArrastar = null;

                        textBotao.text = "[E]";
                        textInfo.text = "Pegar";
                    }
                    else
                    {
                        objArrastar = null;
                        objPegar = null;

                        textBotao.text = "";
                        textInfo.text = "";
                    }
                }
                else
                {
                    objArrastar = null;
                    objPegar = null;

                    textBotao.text = "";
                    textInfo.text = "";
                }
                
               

            }
        }
    }
}
