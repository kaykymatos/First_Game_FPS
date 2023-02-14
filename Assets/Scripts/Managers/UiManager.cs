using Scripts.Personagem;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Managers
{

    public class UiManager : MonoBehaviour
    {

        [Header("Hp e Stamina")]
        public Slider sliderHp;
        public Slider sliderStamina;

        [Header("Script Movimentação")]
        public MovimentaPersonagem scriptMovimenta;

        [Header("UI Sprites")]
        public Text municao;
        public Image imagemModoTiro;
        public Sprite[] modoTiro;

        [Header("Mira")]
        public RectTransform mira;

        void Start()
        {
            scriptMovimenta = GameObject.FindWithTag("Player").GetComponent<MovimentaPersonagem>();
        }

        void Update()
        {
            sliderHp.value = scriptMovimenta.hp;
            sliderStamina.value = scriptMovimenta.stamina;
        }
    }
}