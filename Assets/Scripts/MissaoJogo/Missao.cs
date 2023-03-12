using Scripts.Inimigo;
using UnityEngine;

namespace Scripts.MissaoJogo
{
    public class Missao : MonoBehaviour
    {
        public int numeroCaixas;
        public GameObject boss;
        public bool invocaBoss;
        public Material materialBoss;
        public float valorAmount;

        InimigoRange scriptBoss;
        public static bool bossMorto;

        void Start()
        {
            numeroCaixas = 0;
            invocaBoss = true;
            valorAmount = 0.15f;
            scriptBoss = boss.GetComponent<InimigoRange>();
        }

        void Update()
        {

            if (numeroCaixas == 1 && invocaBoss)
            {
                invocaBoss = false;
                boss.SetActive(true);
            }
            if (!invocaBoss)
            {
                valorAmount = Mathf.Lerp(valorAmount, 0, Time.deltaTime * 2);
                // materialBoss.SetFloat("_Amount", valorAmount);
            }
            bossMorto = scriptBoss.estaMorto;
        }
        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.CompareTag("objArrasta"))
            {
                numeroCaixas++;
            }
        }
        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.CompareTag("objArrasta"))
            {
                numeroCaixas--;
            }
        }
    }
}