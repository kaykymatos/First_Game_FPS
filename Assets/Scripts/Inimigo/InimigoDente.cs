using Scripts.Personagem;
using UnityEngine;
using UnityEngine.AI;

public class InimigoDente : MonoBehaviour
{
    public NavMeshAgent navMesh;
    public GameObject player;
    public float distanciaAtaque;
    public float distanciaPlayer;
    public float velocidade = 5f;
    Animator anim;
    public int hp = 100;
    RegDoll regScript;
    public bool estaMorto;
    public GameObject objetoDesliza;
    public bool bravo;
    public Renderer render;
    public bool invencivel;
    public AudioClip[] sonsMonstro;
    public AudioSource audioMonstro;

    // Start is called before the first frame update
    void Start()
    {
        invencivel = false;
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        regScript = GetComponent<RegDoll>();
        regScript.DesativarRegdoll();
        audioMonstro = GetComponent<AudioSource>();
        render = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!estaMorto)
        {
            if (hp <= 50 && !bravo)
            {
                bravo = true;
                anim.ResetTrigger("levouTiro");
                ParaDeAndar();
                anim.CrossFade("Zombie Scream", 0.2f);
                render.material.color = Color.red;
                velocidade = 8;
            }
            distanciaPlayer = Vector3.Distance(transform.position, player.transform.position);
            VaiAtrasPlayer();
            OlhaParaPlayer();
            if (hp <= 0 && !estaMorto)
            {
                Morre();
                objetoDesliza.SetActive(false);
                estaMorto = true;
                ParaDeAndar();
                regScript.AtivaRegdoll();
            }
        }

    }
    public void DaDanoPlayer()
    {
        player.GetComponent<MovimentaPersonagem>().hp -= 10;
    }
    void ParaDeAndar()
    {
        navMesh.isStopped = true;
        anim.SetBool("podeAndar", false);
        CorrigeRigEntra();

    }
    void OlhaParaPlayer()
    {
        Vector3 direcaoOlha = player.transform.position - transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlha);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 100);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CorrigeRigEntra();

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CorrigeRigSai();
        }
    }
    void CorrigeRigEntra()
    {
        regScript.rigid.isKinematic = true;
        regScript.rigid.velocity = Vector3.zero;
    }
    void CorrigeRigSai()
    {
        regScript.rigid.isKinematic = false;
    }
    void VaiAtrasPlayer()
    {
        navMesh.speed = velocidade;
        if (distanciaPlayer < distanciaAtaque)
        {
            navMesh.isStopped = true;
            anim.SetTrigger("ataca");
            anim.SetBool("podeAndar", false);
            anim.SetBool("paraAtaque", false);
            CorrigeRigEntra();
        }
        if (distanciaPlayer >= 3)
        {

            anim.SetBool("paraAtaque", true);
        }
        if (anim.GetBool("podeAndar"))
        {
            navMesh.isStopped = false;
            navMesh.SetDestination(player.transform.position);
            anim.ResetTrigger("ataca");
            CorrigeRigSai();
        }
    }
    public void LevouDano(int dano)
    {
        int n;
        n = Random.Range(0, 10);
        if (n % 2 == 0 && !bravo)
        {
            anim.SetTrigger("levouTiro");
            ParaDeAndar();
        }
        if (!invencivel)
        {
            hp -= dano;
        }


    }
    public void FicaInvencivel()
    {
        invencivel = true;
    }
    public void SaiInvencivel()
    {
        invencivel = false;
        anim.speed = 2;

    }
    public void PassoMosntro()
    {
        audioMonstro.volume = 0.05f;
        audioMonstro.PlayOneShot(sonsMonstro[0]);
    }
    public void SenteDor()
    {
        audioMonstro.volume = 1f;
        audioMonstro.clip = sonsMonstro[1];
        audioMonstro.Play();
    }

    public void Grito()
    {
        audioMonstro.volume = 1f;
        audioMonstro.clip = sonsMonstro[2];
        audioMonstro.Play();
    }
    public void Morre()
    {
        audioMonstro.volume = 1f;
        audioMonstro.clip = sonsMonstro[3];
        audioMonstro.Play();
    }



}
