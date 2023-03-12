using Scripts.Inimigo;
using Scripts.Managers;
using Scripts.Personagem;
using System.Collections;
using UnityEngine;

namespace Scripts.Armas
{
    public class Glock : MonoBehaviour
    {
        [Header("Animacões")]
        private Animator anim;
        private bool estaAtirando;
        private RaycastHit hit;

        [Header("Efeitos")]
        public GameObject faisca;
        public GameObject buraco;
        public GameObject fumaca;
        public GameObject efeitoTiro;
        public GameObject posEfeitoTiro;
        public GameObject particulaSangue;

        [Header("Particulas")]
        public ParticleSystem rastroBala;

        [Header("Munição")]
        public int carregador = 3;
        public int municao = 17;

        [Header("Audios Arma")]
        public AudioSource audioArma;
        public AudioClip[] sonsArma;

        [Header("Posição Arma")]
        private UiManager uiScript;
        public GameObject posUi;
        private MovimentaArma movimentoArmacript;

        [Header("Verificações")]
        public bool automatico;
        public float numeroAleatorioMira;
        public float valorDaMira;
        bool gancho;
        public int modoDeTiro;
        MovimentaPersonagem scriptPersonagem;

        public ParticleSystem vento;

        Camera cam;

        private void Awake()
        {
            this.gameObject.SetActive(false);
        }
        void Start()
        {
            estaAtirando = false;
            automatico = false;
            anim = GetComponent<Animator>();
            audioArma = GetComponent<AudioSource>();
            uiScript = GameObject.FindGameObjectWithTag("UiManager").GetComponent<UiManager>();
            movimentoArmacript = GetComponentInParent<MovimentaArma>();
            valorDaMira = 300;
            modoDeTiro = 0;
            gancho = false;
            scriptPersonagem = GetComponentInParent<MovimentaPersonagem>();
            cam = Camera.main;

        }
     

        void Update()
        {

            Debug.DrawRay(cam.transform.position, cam.transform.forward * 70, Color.red);
            uiScript.municao.transform.position = cam.WorldToScreenPoint(posUi.transform.position);
            uiScript.municao.text = municao.ToString() + "/" + carregador.ToString();
            ModificaMira();
            if (anim.GetBool("OcorreAcao"))
            {
                return;
            }
            ModoTiro();

            if (!gancho)
            {
                Atira();
                Recarrega();
                Mira();
            }
            else if (gancho)
            {
                AtiraGancho();
                EfeitoPuxando();
            }

        }
        void ModificaFov(float fov)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, Time.deltaTime * 10);

        }
        void AtiraGancho()
        {

            if (Input.GetButtonDown("Fire1"))
            {

                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
                {
                    scriptPersonagem.posicaoHitGancho = hit.point;
                    scriptPersonagem.estado = MovimentaPersonagem.Estado.GanchoIndo;
                }
            }
        }
        void EfeitoPuxando()
        {
            if (scriptPersonagem.estado == MovimentaPersonagem.Estado.GanchoPuxando)
            {
                ModificaFov(85);
                vento.Emit(1);
            }
            else
            {
                ModificaFov(60);
                vento.Stop();
            }
        }
        void ModoTiro()
        {
            if (Input.GetKeyDown(KeyCode.Q)&&scriptPersonagem.estado == MovimentaPersonagem.Estado.Normal)
            {
                audioArma.clip = sonsArma[2];
                audioArma.Play();
                automatico = !automatico;
                if (modoDeTiro == 0)
                {
                    automatico = true;
                    modoDeTiro++;
                    uiScript.imagemModoTiro.sprite = uiScript.modoTiro[1];
                }
                else if (modoDeTiro == 1)
                {
                    automatico = false;
                    gancho = true;
                    modoDeTiro++;
                    uiScript.imagemModoTiro.sprite = uiScript.modoTiro[2];
                }
                else
                {
                    gancho = false;
                    uiScript.imagemModoTiro.sprite = uiScript.modoTiro[0];
                    modoDeTiro = 0;
                }
            }
        }
        void Atira()
        {
            if (Input.GetButtonDown("Fire1") || automatico ? Input.GetButton("Fire1") : false)
            {
                if (!estaAtirando && municao > 0)
                {
                    municao--;
                    estaAtirando = true;
                    StartCoroutine(Atirando());
                    SomAtira();

                }
                else if (!estaAtirando && municao == 0 && carregador > 0)
                {
                    anim.Play("Recarrega");
                    carregador--;
                    municao = 17;
                    SomRecarrega();
                }
                else if (municao == 0 && carregador == 0)
                {
                    SomSemMunicao();
                }
            }
        }
        void Recarrega()
        {
            if (Input.GetKeyDown(KeyCode.R) && carregador > 0 && municao < 17)
            {
                anim.Play("Recarrega");
                carregador--;
                municao = 17;
                SomRecarrega();
            }
        }
        void ModificaMira()
        {
            if (estaAtirando)
            {
                valorDaMira = Mathf.Lerp(valorDaMira, 450, Time.deltaTime * 20);
                uiScript.mira.sizeDelta = new Vector2(valorDaMira, valorDaMira);
            }
            else
            {
                valorDaMira = Mathf.Lerp(valorDaMira, 300, Time.deltaTime * 20);
                uiScript.mira.sizeDelta = new Vector2(valorDaMira, valorDaMira);
            }
        }
        void Mira()
        {
            if (Input.GetButton("Fire2"))
            {
                numeroAleatorioMira = 0f;
                anim.SetBool("Mira", true);
                movimentoArmacript.valor = 0.01f;
                uiScript.mira.gameObject.SetActive(false);
                posUi.transform.localPosition = new Vector3(0f, 0.1f, -0.2f);
                ModificaFov(45);

            }
            else
            {
                numeroAleatorioMira = 0.05f;
                movimentoArmacript.valor = 0.1f;
                uiScript.mira.gameObject.SetActive(true);
                anim.SetBool("Mira", false);
                posUi.transform.localPosition = new Vector3(-0.02f, 0.1f, -0.2f);
                ModificaFov(60);
            }
        }
        void InimigoVerificadorDano()
        {
            if (hit.transform.GetComponent<InimigoDente>())
            {
                hit.transform.GetComponent<InimigoDente>().LevouDano(15);
            }
            else if (hit.transform.GetComponent<InimigoRange>())
            {
                hit.transform.GetComponent<InimigoRange>().LevouDano(15);
            }
        }
        IEnumerator Atirando()
        {
            float screenX = Screen.width / 2;
            float screenY = Screen.height / 2;

            Ray ray = cam.ScreenPointToRay(new Vector3(screenX, screenY));
            anim.Play("Atira");

            GameObject efeitoTiroObjeto = Instantiate(efeitoTiro, posEfeitoTiro.transform.position, posEfeitoTiro.transform.rotation);
            efeitoTiroObjeto.transform.parent = posEfeitoTiro.transform;


            if (Physics.Raycast(new Vector3(ray.origin.x + Random.Range(-numeroAleatorioMira, numeroAleatorioMira), ray.origin.y + Random.Range(-numeroAleatorioMira, numeroAleatorioMira), ray.origin.z), cam.transform.forward, out hit))
            {
                if (hit.transform.CompareTag("inimigo"))
                {
                    if (hit.transform.GetComponent<InimigoDente>() || hit.transform.GetComponent<InimigoRange>())
                    {
                        InimigoVerificadorDano();
                    }
                    else if (hit.rigidbody != null && hit.transform.GetComponent<InimigoRange>())
                    {
                        AdicionaForca(ray, 900);
                    }

                    GameObject particulaCriada = Instantiate(particulaSangue, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                    particulaCriada.transform.parent = hit.transform;
                }
                else
                {
                    InstanciaEfeitos();
                    if (hit.rigidbody != null)
                    {
                        AdicionaForca(ray, 400);
                    }
                }

            }

            yield return new WaitForSeconds(0.3f);
            estaAtirando = false;
            audioArma.Stop();
        }
        void AdicionaForca(Ray ray, float forca)
        {
            Vector3 direcaoBala = ray.direction;
            hit.rigidbody.AddForceAtPosition(direcaoBala * forca, hit.point);
        }
        void InstanciaEfeitos()
        {
            Instantiate(faisca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            Instantiate(fumaca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            GameObject buracoObj = Instantiate(buraco, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

            buracoObj.transform.parent = hit.transform;
        }
        void SomRecarrega()
        {
            audioArma.clip = sonsArma[1];
            audioArma.Play();
        }
        void SomAtira()
        {
            audioArma.clip = sonsArma[0];
            audioArma.Play();
        }
        void SomSemMunicao()
        {
            audioArma.clip = sonsArma[2];
            audioArma.Play();
        }
    }
}
