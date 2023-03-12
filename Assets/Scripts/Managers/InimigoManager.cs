using Scripts.MissaoJogo;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Managers
{
    public class InimigoManager : MonoBehaviour
    {
        public GameObject[] pontos;
        public GameObject[] inimigos;
        public float tempo;
        public List<GameObject> listaInimigos = new();
        Missao missaoScript;

        // Start is called before the first frame update
        void Start()
        {
            tempo = 0;
            CriaInimigos();
        }

        void Update()
        {
            if (!Missao.bossMorto)
            {
                tempo += Time.deltaTime;
                if (tempo > 60)
                {
                    tempo = 0;

                    for (int i = 0; i < listaInimigos.Count; i++)
                    {
                        if (listaInimigos[i] == null)
                        {
                            listaInimigos.RemoveAt(i);
                        }
                    }
                    if (listaInimigos.Count < 20)
                    {
                        CriaInimigos();
                    }


                }

            }
            else if (Missao.bossMorto)
            {
                if (listaInimigos.Count > 0)
                {
                    for (int i = 0; i < listaInimigos.Count; i++)
                    {
                        Destroy(listaInimigos[i]);
                        listaInimigos.RemoveAt(i);
                    }
                }
            }
        }
        void CriaInimigos()
        {
            for (int i = 0; i < pontos.Length; i++)
            {
                int random = Random.Range(0, 10);
                if (random > 5)
                {
                    GameObject inimigo = Instantiate(inimigos[0], pontos[i].transform.position, Quaternion.identity);
                    listaInimigos.Add(inimigo);
                }
                else
                {
                    GameObject inimigo = Instantiate(inimigos[1], pontos[i].transform.position, Quaternion.identity);
                    listaInimigos.Add(inimigo);
                }
            }
        }
    }
}