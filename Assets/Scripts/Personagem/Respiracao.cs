using UnityEngine;

namespace Scripts.Personagem
{
    public class Respiracao : MonoBehaviour
    {
        private float movimento;

        [Header("Verificação")]
        public bool estaInspirando = true;

        [Header("Valores Movimento")]
        public float minAltura = -0.035f;
        public float maxAltura = 0.035f;

        [Range(0f, 5f)]
        public float forcaResp = 1f;

        void Update()
        {
            if (estaInspirando)
            {
                movimento = Mathf.Lerp(movimento, maxAltura, Time.deltaTime * 1 * forcaResp);
                transform.localPosition = new Vector3(transform.localPosition.x, movimento, transform.localPosition.z);
                if (movimento >= maxAltura - 0.01f)
                {
                    estaInspirando = !estaInspirando;
                }
            }
            else
            {
                movimento = Mathf.Lerp(movimento, minAltura, Time.deltaTime * 1 * forcaResp);
                transform.localPosition = new Vector3(transform.localPosition.x, movimento, transform.localPosition.z);
                if (movimento <= minAltura + 0.01f)
                {
                    estaInspirando = !estaInspirando;
                }
            }

            if (forcaResp != 0)
            {
                forcaResp = Mathf.Lerp(forcaResp, 1f, Time.deltaTime * 0.2f);
            }

        }
    }
}