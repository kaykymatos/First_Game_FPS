using Scripts.Managers;
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
        private Vector3 velocidadeCai;
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

        [Header("Sons")]
        public AudioClip[] audiosGerais;
        private AudioSource audioPersonagem;
        private bool noAr;

        [Header("Trava pulo")]
        public Vector3 pontoContato;
        public bool podePular;
        RaycastHit hitContato;
        public float anguloLimitePulo;
        public float distanciaRaio;

        [Header("Gancho")]
        public Vector3 posicaoHitGancho;
        public Estado estado;
        Vector3 velocidadeMomentanea;
        public GameObject ganchoObj;
        public GameObject paiGancho;

        public enum Estado
        {
            Normal,
            GanchoPuxando,
            GanchoIndo,
        }

        UiManager uiScript;

        void Start()
        {
            controle = GetComponent<CharacterController>();
            estaAbaixado = false;
            estaCorrendo = false;
            stamina = 100f;
            cansado = false;
            hp = 100f;
            cameraTransform = Camera.main.transform;
            audioPersonagem = GetComponent<AudioSource>();
            noAr = false;
            uiScript = GameObject.FindGameObjectWithTag("UiManager").GetComponent<UiManager>();
            estado = Estado.Normal;
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            pontoContato = hit.point;
        }
        void FixedUpdate()
        {
            if (Physics.Raycast(transform.position, -Vector3.up, out hitContato))
            {
                if (Vector3.Angle(hitContato.normal, Vector3.up) > anguloLimitePulo)
                {
                    podePular = false;
                }
                else
                {
                    podePular = true;
                }
            }
        }


        void Update()
        {
            switch (estado)
            {
                case Estado.Normal:
                    Movimenta();
                    MovimentoAbaixa();
                    Inputs();
                    CondicaoDoPlayer();
                    SomPulo();
                    paiGancho.SetActive(false);
                    ganchoObj.transform.parent = paiGancho.transform;
                    ganchoObj.transform.position = Vector3.zero;
                    break;
                case Estado.GanchoIndo:
                    ganchoObj.transform.parent = null;
                    paiGancho.SetActive(true);
                    Movimenta();
                    GanchoMovimentando();
                    break;
                case Estado.GanchoPuxando:
                    MovimentaPersonagemGancho();
                    break;

            }

        }
        void MovimentaPersonagemGancho()
        {
            float velocidadeMin = 10f;
            float velocidadeMax = 40f;
            Vector3 direcao = (posicaoHitGancho - transform.position).normalized;
            float puxaVel = Mathf.Clamp(Vector3.Distance(transform.position, posicaoHitGancho), velocidadeMin, velocidadeMax);
            controle.Move(direcao * puxaVel * Time.deltaTime * 2);

            if (Vector3.Distance(transform.position, posicaoHitGancho) < 2)
            {
                estado = Estado.Normal;
                paiGancho.SetActive(false);
            }
            if (RetornaPulo())
            {
                estado = Estado.Normal;
                velocidadeMomentanea = (transform.forward*5) + transform.up*7;
                paiGancho.SetActive(false);

            }
        }
        bool RetornaPulo()
        {
            return Input.GetButtonDown("Jump");
        }
        void GanchoMovimentando()
        {
            ganchoObj.transform.LookAt(posicaoHitGancho);
            ganchoObj.transform.position = Vector3.MoveTowards(ganchoObj.transform.position, posicaoHitGancho, 50 * Time.deltaTime);

            if (ganchoObj.transform.position == posicaoHitGancho)
            {
                estado = Estado.GanchoPuxando;
            }
        }
        void Abaixa()
        {
            if (levantarBloqueado || !estaNoChao)
                return;

            estaAbaixado = !estaAbaixado;

        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("cabecaDesliza"))
            {
                controle.SimpleMove(transform.forward * 1000 * Time.deltaTime);
            }
        }
        void SomPulo()
        {
            if (!estaNoChao)
            {
                noAr = true;
            }
            if (estaNoChao && noAr)
            {
                noAr = false;
                audioPersonagem.clip = audiosGerais[1];
                audioPersonagem.Play();
            }
        }
        void Movimenta()
        {
            estaNoChao = Physics.CheckSphere(checaChao.position, raioEsfera, chaoMask);

            if (estaNoChao && velocidadeCai.y < 0)
            {
                velocidadeCai.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = (transform.right * x + transform.forward * z).normalized;

            if (velocidadeMomentanea.magnitude >= 0f)
            {
                float movimentoPuxa = 3;
                velocidadeMomentanea -= velocidadeMomentanea * movimentoPuxa * Time.deltaTime;

                if (velocidadeMomentanea.magnitude <= 0.1f)
                {
                    velocidadeMomentanea = Vector3.zero;
                }
            }
            move += velocidadeMomentanea;

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
            if (RetornaPulo() && estaNoChao && podePular)
            {
                velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
                audioPersonagem.clip = audiosGerais[0];
                audioPersonagem.Play();
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
        public void LevouDano(int dano)
        {
            hp -= dano;

            audioPersonagem.clip = audiosGerais[2];
            audioPersonagem.Play();
            uiScript.imgMachuca.GetComponent<Animator>().Play("MachucaImg");
        }
    }
}