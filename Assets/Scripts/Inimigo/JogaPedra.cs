using Scripts.Personagem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogaPedra : MonoBehaviour
{
    Rigidbody rigid;
    public float hVelocidade = 15;
    public float vVelocidade = 4;
    GameObject player;
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
        rigid.AddForce(targetForca,ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<MovimentaPersonagem>().hp -= 30;
            Destroy(this.gameObject);
        }
    }
}
