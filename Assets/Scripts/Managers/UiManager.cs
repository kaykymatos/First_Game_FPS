using Scripts.Personagem;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Slider sliderHp;
    public Slider sliderStamina;
    public MovimentaPersonagem scriptMovimenta;
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
