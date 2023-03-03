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

        void Start()
        {
            rigid = GetComponent<Rigidbody>();
            navMesh = GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag("Player");
            anim = GetComponent<Animator>();
            estaMorto = false;
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
                    estaMorto = true;
                    navMesh.isStopped = true;
                    navMesh.enabled = false;
                    CorrigiRigEntra();
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
            pedraPermanete.SetActive(true);
        }

        void VaiAtrasPlayer()
        {
            navMesh.speed = velocidade;
            if (distanciaPlayer < distanciaAtaque)
            {
                navMesh.isStopped = true;
                anim.SetBool("joga", true);
                CorrigiRigEntra();
            }
            else
            {
                navMesh.isStopped = false;
                anim.SetBool("joga", false);
                navMesh.SetDestination(player.transform.position);
                CorrigeRigSai();
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
    }
}