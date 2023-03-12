using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Inimigo
{
    public class InimigoRange : MonoBehaviour
    {
        public NavMeshAgent navMesh;
        public GameObject player;
        public float distanciaAtaque;
        public float distanciaPlayer;
        public float velocidade = 5;
        Animator anim;
        public int hp = 100;
        public bool estaMorto;
        public Rigidbody rigid;

        public GameObject pedraPermanete;
        public GameObject pedraInstanciada;

        public bool usaCurvaAnimacao;
        public CapsuleCollider col;
        public GameObject objetoDesliza;
        public AudioClip[] sonsMonstro;
        public AudioSource audioMonstro;

        void Start()
        {
            usaCurvaAnimacao = false;
            rigid = GetComponent<Rigidbody>();
            navMesh = GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag("Player");
            anim = GetComponent<Animator>();
            audioMonstro = GetComponent<AudioSource>();
            estaMorto = false;
            col = GetComponent<CapsuleCollider>();
        }

        void Update()
        {

            if (!estaMorto)
            {
                distanciaPlayer = Vector3.Distance(transform.position, player.transform.position);

                VaiAtrasPlayer();
                OlhaParaPlayer();
                if (hp <= 0)
                {
                    StartCoroutine(SomeMorto());

                    estaMorto = true;
                    navMesh.isStopped = true;
                    navMesh.enabled = false;
                    CorrigiRigEntra();
                    anim.CrossFade("Zombie Death", 0.2f);
                    transform.gameObject.layer = 10;
                    anim.applyRootMotion = true;
                    col.direction = 2;
                    usaCurvaAnimacao = false;
                    objetoDesliza.SetActive(false);
                    Morre();
                    GetComponent<DropItem>().Dropa();
                    pedraPermanete.SetActive(false);

                }
                if (usaCurvaAnimacao && anim.IsInTransition(0))
                {
                    col.height = anim.GetFloat("alturaCollider");
                    col.center = new Vector3(0, anim.GetFloat("centroColliderY"), 0);
                }
                else
                {
                    col.height = 2;
                    col.center = new Vector3(0, 1, 0);
                }
            }
        }
        public void InstanciaPedra()
        {
            pedraPermanete.SetActive(false);
            GameObject pedra = Instantiate(pedraInstanciada, anim.GetBoneTransform(HumanBodyBones.RightThumbIntermediate).transform);
            pedra.transform.parent = null;
            pedra.transform.LookAt(player.transform.position);

            JogaPedra jogaScript = pedra.GetComponent<JogaPedra>();
            jogaScript.Jogar();
        }
        public void AparecePedraPermanente()
        {
            if (!estaMorto)
                pedraPermanete.SetActive(true);
        }

        void VaiAtrasPlayer()
        {
            if (!estaMorto)
            {


                navMesh.speed = velocidade;
                if (distanciaPlayer < distanciaAtaque)
                {
                    navMesh.isStopped = true;
                    anim.SetBool("joga", true);
                    CorrigiRigEntra();
                    usaCurvaAnimacao = true;
                }
                else
                {
                    pedraPermanete.SetActive(false);
                    navMesh.isStopped = false;
                    anim.SetBool("joga", false);
                    navMesh.SetDestination(player.transform.position);
                    CorrigeRigSai();
                    usaCurvaAnimacao = false;
                }
            }
        }
        void OlhaParaPlayer()
        {
            Vector3 direcaoOlha = player.transform.position - transform.position;
            Quaternion rotacao = Quaternion.LookRotation(direcaoOlha);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 100);
        }
        void CorrigiRigEntra()
        {
            rigid.isKinematic = true;
            rigid.velocity = Vector3.zero;
        }
        void CorrigeRigSai()
        {
            rigid.isKinematic = false;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                CorrigiRigEntra();

            }
        }
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                CorrigeRigSai();
            }
        }
        public void LevouDano(int dano)
        {
            hp -= dano;
        }
        public void Morre()
        {
            audioMonstro.volume = 1f;
            audioMonstro.clip = sonsMonstro[1];
            audioMonstro.Play();
        }
        public void PassoMosntro()
        {
            audioMonstro.volume = 0.05f;
            audioMonstro.PlayOneShot(sonsMonstro[0]);
        }
        IEnumerator SomeMorto()
        {
            yield return new WaitForSeconds(10);
            col.enabled = false;
            rigid.isKinematic = false;
            anim.enabled = false;
            yield return new WaitForSeconds(3);
            Destroy(this.gameObject);
        }
    }
}