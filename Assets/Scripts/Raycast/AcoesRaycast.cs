using Scripts.Armas;
using Scripts.Personagem;
using UnityEngine;

namespace Scripts.Raycast
{
    [RequireComponent(typeof(RaycastScript))]
    public class AcoesRaycast : MonoBehaviour
    {
        private RaycastScript raycastScript;
        private float distancia;

        [Header("Verificação")]
        public bool pegou;

        [Header("Objeto")]
        public GameObject salvaObjeto;

        MovimentaPersonagem scriptMovimenta;

        void Start()
        {
            raycastScript = GetComponent<RaycastScript>();
            pegou = false;
            scriptMovimenta = GetComponentInParent<MovimentaPersonagem>();
        }

        void Update()
        {
            distancia = raycastScript.distanciaAlvo;
            if (distancia <= 3)
            {
                if (Input.GetKeyDown(KeyCode.E) && raycastScript.objPegar != null)
                {
                    Pegar();
                }
                if (Input.GetKeyDown(KeyCode.E) && raycastScript.objArrastar != null)
                {
                    if (!pegou)
                    {
                        pegou = true;
                        Arrastar();
                    }
                    else
                    {
                        pegou = false;
                        Soltar();
                    }
                }
            }
        }
        void Pegar()
        {
            scriptMovimenta.hp += 50;
            scriptMovimenta.hp = Mathf.Clamp(scriptMovimenta.hp, 0, 100);
            GetComponentInChildren<Glock>().carregador = 3;
            Destroy(raycastScript.objPegar);
        }
        void Arrastar()
        {
            raycastScript.objArrastar.GetComponent<Rigidbody>().isKinematic = true;
            raycastScript.objArrastar.GetComponent<Rigidbody>().useGravity = false;
            raycastScript.objArrastar.transform.SetParent(transform);
            raycastScript.objArrastar.transform.localPosition = new Vector3(0, 0, 1.5f);
            raycastScript.objArrastar.transform.localRotation = Quaternion.Euler(0, 0, 0);

        }
        void Soltar()
        {
            raycastScript.objArrastar.transform.localPosition = new Vector3(0, 0, 1.5f);
            raycastScript.objArrastar.transform.SetParent(null);
            raycastScript.objArrastar.GetComponent<Rigidbody>().isKinematic = false;
            raycastScript.objArrastar.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}