using Scripts.Personagem;
using UnityEngine;

namespace Scripts.Inimigo
{
    public class JogaPedra : MonoBehaviour
    {
        Rigidbody rigid;
        public float hVelocidade = 15;
        public float vVelocidade = 4;
        GameObject player;
        public GameObject somCoque;
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            Destroy(this.gameObject, 8);

        }

        public void Jogar()
        {
            rigid = GetComponent<Rigidbody>();
            Vector3 targetForca = transform.forward * hVelocidade;
            targetForca += transform.up * vVelocidade;
            rigid.AddForce(targetForca, ForceMode.Impulse);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                player.GetComponent<MovimentaPersonagem>().hp -= 30;
                player.GetComponent<MovimentaPersonagem>().SomDano();
            }
            CriaSomChoque();

        }
        void CriaSomChoque()
        {
            GameObject som = Instantiate(somCoque, transform);
            som.transform.parent = null;
            Destroy(this.gameObject);
            Destroy(som, 2);
        }
    }
}