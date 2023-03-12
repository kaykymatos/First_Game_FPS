using UnityEngine;

public class Missao : MonoBehaviour
{
    public int numeroCaixas;
    public GameObject boss;
    public bool invocaBoss;

    // Start is called before the first frame update
    void Start()
    {
        numeroCaixas = 0;
        invocaBoss = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (numeroCaixas > 0 && invocaBoss)
        {
            invocaBoss = false;
            boss.SetActive(true);
        }
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
