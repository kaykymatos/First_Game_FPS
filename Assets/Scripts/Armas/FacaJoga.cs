using Scripts.Inimigo;
using UnityEngine;

namespace Scripts.Armas
{
    public class FacaJoga : MonoBehaviour
    {
        public float velocidade = 1000;
        public float forca = 20;
        Rigidbody rig;
        public GameObject sangue;
        public GameObject som;

        void Start()
        {
            Joga();
        }

        void Joga()
        {
            rig = GetComponent<Rigidbody>();
            transform.eulerAngles = new Vector3(0, 90 + Camera.main.transform.eulerAngles.y, 0);
            Vector3 direcao = Camera.main.transform.forward * forca + transform.up * 4 - (Camera.main.transform.right / 2.5f);
            rig.AddForce(direcao, ForceMode.Impulse);
        }
        void Update()
        {
            transform.eulerAngles += Time.deltaTime * velocidade * Vector3.forward;
        }
        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.CompareTag("inimigo"))
            {
                if (col.transform.gameObject.GetComponent<InimigoDente>())
                    col.transform.gameObject.GetComponent<InimigoDente>().LevouDano(20);

                if (col.transform.gameObject.GetComponent<InimigoRange>())
                    col.transform.gameObject.GetComponent<InimigoRange>().LevouDano(20);

                Instantiate(sangue, transform.position, Quaternion.Euler(0, 90, 0));
                GameObject somFaca = Instantiate(som, transform.position, Quaternion.Euler(0, 0, 0));
                Destroy(somFaca, 2);
            }
            Destroy(this.gameObject);
        }
    }
}