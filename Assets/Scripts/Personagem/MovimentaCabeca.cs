using UnityEngine;

namespace Scripts.Personagem
{
    public class MovimentaCabeca : MonoBehaviour
    {
        [Header("Valores(Força, velocidade, tempo...)")]
        private float tempo = 0.0f;
        public float velocidade = 0.15f;
        public float forca = 0.1f;
        public float pontoDeOrigem = 0.0f;

        [Header("Valores Posição")]
        private float cortaOnda;
        private float horizontal;
        private float vertical;
        private Vector3 salvaPosicao;

        [Header("Audios")]
        private AudioSource audioSourse;
        public AudioClip[] audioClip;
        public int indexPassos;
        private MovimentaPersonagem scriptMovimenta;

        private void Start()
        {
            audioSourse = GetComponent<AudioSource>();
            scriptMovimenta = GetComponentInParent<MovimentaPersonagem>();
            indexPassos = 0;
        }

        void Update()
        {
            cortaOnda = 0.0f;
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            salvaPosicao = transform.localPosition;

            if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            {
                tempo = 0.0f;
            }
            else
            {
                cortaOnda = Mathf.Sin(tempo);
                tempo += velocidade;
                if (tempo > Mathf.PI * 2)
                {
                    tempo -= (Mathf.PI * 2);
                }
            }

            if (cortaOnda != 0)
            {
                float mudaMovimentacao = cortaOnda * forca;
                float eixosTotais = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                eixosTotais = Mathf.Clamp(eixosTotais, 0.0f, 1.0f);
                mudaMovimentacao = eixosTotais * mudaMovimentacao;
                salvaPosicao.y = pontoDeOrigem + mudaMovimentacao;
            }
            else
            {
                salvaPosicao.y = pontoDeOrigem;
            }
            transform.localPosition = salvaPosicao;
            SomPassos();
            AtualizaCabeca();
        }
        void SomPassos()
        {
            if (cortaOnda <= -0.95f && !audioSourse.isPlaying && scriptMovimenta.estaNoChao)
            {
                audioSourse.clip = audioClip[indexPassos];
                audioSourse.Play();
                indexPassos++;
                if (indexPassos >= 4)
                {
                    indexPassos = 0;
                }
            }
        }
        void AtualizaCabeca()
        {
            if (scriptMovimenta.estaCorrendo)
            {
                velocidade = 0.25f;
                forca = 0.25f;
            }
            else if (scriptMovimenta.estaAbaixado)
            {
                velocidade = 0.15f;
                forca = 0.11f;
            }
            else
            {
                velocidade = 0.18f;
                forca = 0.15f;
            }
        }
    }
}