using UnityEngine;

namespace Scripts.Personagem
{
    public class MovimentaPersonagem : MonoBehaviour
    {
        [Header("Configurações Personagem")]
        public CharacterController controle;
        public float velocidade = 6f;
        public float alturaPulo = 3f;
        public float gravidade = -20f;
        public bool estaCorrendo;

        [Header("Verifica Chão")]
        public Transform checaChao;
        public float raioEsfera = 0.4f;
        public LayerMask chaoMask;
        public bool estaNoChao;

        [Header("Verifica Abaixado")]
        Vector3 velocidadeCai;
        public Transform cameraTransform;
        public bool estaAbaixado;
        public bool levantarBloqueado;
        public float alturaLevantado;
        public float alturaAbaixado;
        public float posicaCameraEmPe;
        public float posicaoCameraAbaixado;
        public float velocidadeCorrente;
        RaycastHit hit;

        [Header("HP E stamina")]
        public float stamina;
        public float hp;
        public bool cansado;
        public Respiracao scriptResp;
        void Start()
        {
            controle = GetComponent<CharacterController>();
            estaAbaixado = false;
            estaCorrendo = false;
            stamina = 100f;
            cansado = false;
            hp = 100f;
            cameraTransform = Camera.main.transform;
        }


        void Update()
        {
            Verificacoes();
            MovimentoAbaixa();
            Inputs();
            CondicaoDoPlayer();
        }
        void Abaixa()
        {
            if (levantarBloqueado || !estaNoChao)
                return;

            estaAbaixado = !estaAbaixado;

        }
        void Verificacoes()
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


            velocidadeCai.y += gravidade * Time.deltaTime;
            controle.Move(velocidadeCai * Time.deltaTime);


        }
        void MovimentoAbaixa()
        {
            controle.center = Vector3.down * (alturaLevantado - controle.height) / 2f;
            if (estaAbaixado)
            {
                controle.height = Mathf.Lerp(controle.height, alturaAbaixado, Time.deltaTime * 3);
                float novoY = Mathf.SmoothDamp(cameraTransform.localPosition.y, posicaoCameraAbaixado, ref velocidadeCorrente, Time.deltaTime * 3);
                cameraTransform.localPosition = new Vector3(0, novoY, 0);
                velocidade = 3f;
                ChecaBloqueioAbaixado();
            }
            else
            {
                controle.height = Mathf.Lerp(controle.height, alturaLevantado, Time.deltaTime * 3);
                float novoY = Mathf.SmoothDamp(cameraTransform.localPosition.y, posicaCameraEmPe, ref velocidadeCorrente, Time.deltaTime * 3);
                cameraTransform.localPosition = new Vector3(0, novoY, 0);
            }
        }
        void Inputs()
        {
            if (Input.GetKey(KeyCode.LeftShift) && estaNoChao && !estaAbaixado && !cansado)
            {
                estaCorrendo = true;
                velocidade = 9f;
                stamina -= 0.3f;
                stamina = Mathf.Clamp(stamina, 0f, 100f);
            }
            else
            {
                estaCorrendo = false;
                velocidade = 6f;
                stamina += 0.1f;
                stamina = Mathf.Clamp(stamina, 0f, 100f);
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Abaixa();
            }
            if (Input.GetButtonDown("Jump") && estaNoChao)
            {
                velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            }
        }
        void CondicaoDoPlayer()
        {
            if (stamina == 0)
            {
                cansado = true;
                scriptResp.forcaResp = 5f;
            }
            if (stamina > 20)
            {
                cansado = false;
            }
        }
        void ChecaBloqueioAbaixado()
        {
            Debug.DrawRay(cameraTransform.position, Vector3.up * 1.1f, Color.red);

            if (Physics.Raycast(cameraTransform.position, Vector3.up, out hit, 1.1f))
            {
                levantarBloqueado = true;
            }
            else
            {
                levantarBloqueado = false;
            }
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(checaChao.position, raioEsfera);
        }
    }
}