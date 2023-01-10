using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS_First_Game.Assets.Scripts
{
    public class MovimentaPersonagem : MonoBehaviour
    {
        public CharacterController controle;
        public float velocidade = 6f;
        public float alturaPulo = 3f;
        public float gravidade = -20f;

        public Transform checaChao;
        public float raioEsfera = 0.4f;
        public LayerMask chaoMask;
        public bool estaNoChao;

        Vector3 velocidadeCai;

        // Start is called before the first frame update
        void Start()
        {
            controle = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            estaNoChao = Physics.CheckSphere(checaChao.position, raioEsfera, chaoMask);

            if (estaNoChao && velocidadeCai.y < 0)
            {
                velocidadeCai.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = (transform.right * x + transform.forward * z).normalized;

            controle.Move(move * velocidade * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && estaNoChao)
            {
                velocidadeCai.y = Mathf.Sqrt(alturaPulo*-2f*gravidade);
            }
            velocidadeCai.y += gravidade * Time.deltaTime;
            controle.Move(velocidadeCai * Time.deltaTime);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(checaChao.position, raioEsfera);
        }
    }

}
