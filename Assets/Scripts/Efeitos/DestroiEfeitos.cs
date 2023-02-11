using UnityEngine;

namespace Scripts.Efeitos
{

    public class DestroiEfeitos : MonoBehaviour
    {

        [Header("Tempo")]
        public float tempo = 0;
        void Start()
        {
            Destroy(this.gameObject, tempo);
        }
    }

}