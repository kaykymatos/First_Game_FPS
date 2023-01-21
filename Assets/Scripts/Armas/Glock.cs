using System.Collections;
using UnityEngine;

namespace Scripts.Armas
{
    public class Glock : MonoBehaviour
    {
        Animator anim;
        bool estaAtirando;
        RaycastHit hit;

        public GameObject faisca;
        public GameObject buraco;
        public GameObject fumaca;
        public GameObject efeitoTiro;
        public GameObject posEfeitoTiro;

        void Start()
        {
            estaAtirando = false;
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            if (Input.GetButton("Fire1"))
            {
                if (!estaAtirando)
                {
                    estaAtirando = true;
                    StartCoroutine(Atirando());
                }
            }
        }
        IEnumerator Atirando()
        {
            float screenX = Screen.width / 2;
            float screenY = Screen.height / 2;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY));
            anim.Play("Atira");

            GameObject efeitoTiroObjeto = Instantiate(efeitoTiro, posEfeitoTiro.transform.position, posEfeitoTiro.transform.rotation);
            efeitoTiroObjeto.transform.parent = posEfeitoTiro.transform;


            if (Physics.SphereCast(ray, 0.1f, out hit))
            {
                InstanciaEfeitos();
                if (hit.transform.CompareTag("objArrasta"))
                {
                    Vector3 direcaoBala = ray.direction;
                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForceAtPosition(direcaoBala * 500, hit.point);
                    }
                }
            }

            yield return new WaitForSeconds(0.3f);
            estaAtirando = false;
        }
        void InstanciaEfeitos()
        {
            Instantiate(faisca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            Instantiate(fumaca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            GameObject buracoObj = Instantiate(buraco, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

            buracoObj.transform.parent = hit.transform;
        }
    }
}
