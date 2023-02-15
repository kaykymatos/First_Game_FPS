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

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        regScript = GetComponent<RegDoll>();
        regScript.DesativarRegdoll();
    }

    // Update is called once per frame
    void Update()
    {
        distanciaPlayer = Vector3.Distance(transform.position, player.transform.position);
        VaiAtrasPlayer();
        OlhaParaPlayer();
        if(hp<=0 && !estaMorto)
        {
            estaMorto = true;
            ParaDeAndar();
            regScript.AtivaRegdoll();

            this.enabled = false;
        }
    }
    void ParaDeAndar()
    {
        navMesh.isStopped = true;
        anim.SetBool("podeAndar", false) ;

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
        hp -= dano;
    }
}
