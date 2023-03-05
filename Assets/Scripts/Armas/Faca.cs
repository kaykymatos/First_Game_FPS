using UnityEngine;

namespace Scripts.Armas
{
    public class Faca : MonoBehaviour
    {
        Animator anim;
        public GameObject facaJoga;
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                anim.Play("AtiraFaca");
            }
        }
        public void JogaFaca()
        {
            GameObject faca = Instantiate(facaJoga, transform);
            faca.transform.parent = null;
        }
    }
}