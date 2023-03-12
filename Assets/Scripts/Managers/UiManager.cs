using Scripts.MissaoJogo;
using Scripts.Personagem;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public Sprite[] spriteItens;
        public Image iconeImagem;
        public Image imgMachuca;

        [Header("Mira")]
        public RectTransform mira;

        public bool fimJogo;
        public Text txtFrase;
        public Button[] botoes;
        Missao missaoScript;

        float tempo;

        void Start()
        {
            missaoScript = GameObject.FindGameObjectWithTag("missao").GetComponent<Missao>();

            scriptMovimenta = GameObject.FindWithTag("Player").GetComponent<MovimentaPersonagem>();
            fimJogo = false;
        }

        void Update()
        {
            sliderHp.value = scriptMovimenta.hp;
            sliderStamina.value = scriptMovimenta.stamina;
            if (scriptMovimenta.hp <= 0 && !fimJogo)
            {
                tempo = 0;
                txtFrase.text = "Você Perdeu!!!";
                StartCoroutine(nameof(FimDoJogo));
            }
            else if (Missao.bossMorto && !fimJogo)
            {
                tempo = 5;
                txtFrase.text = "Você Ganhou!!!";
                StartCoroutine(nameof(FimDoJogo));
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        IEnumerator FimDoJogo()
        {
            yield return new WaitForSeconds(tempo);
            fimJogo = true;
            imgMachuca.GetComponent<Animator>().Play("FimJogo");
            Time.timeScale = 0;
            Camera.main.gameObject.GetComponent<AudioListener>().enabled = false;
            for (int i = 0; i < botoes.Length; i++)
                botoes[i].gameObject.SetActive(true);

            txtFrase.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        public void ReiniaJogo()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }
        public void SaiDoJogo()
        {
            Application.Quit();
        }
    }
}