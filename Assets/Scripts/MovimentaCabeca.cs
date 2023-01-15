using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentaCabeca : MonoBehaviour
{
    private float tempo = 0.0f;
    public float velocidade = 0.1f;
    public float forca = 0.2f;
    public float pontoDeOrigem = 0.0f;

    float cortaOnda;
    float horizontal;
    float vertical;
    Vector3 salvaPosicao;

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
    }
}
