using UnityEngine;

namespace Scripts.Personagem
{
    public class RotacaoCamera : MonoBehaviour
    {
        [Header("Valores Sensibilidade, angulos, rotação")]
        public float sensibilidadeMouse = 100f;
        public float anguloMin = -45f;
        public float anguloMax = 45f;
        private float rotacao = 0f;

        [Header("Player")]
        public Transform transformPlayer;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse * Time.deltaTime;

            rotacao -= mouseY;
            rotacao = Mathf.Clamp(rotacao, anguloMin, anguloMax);
            transform.localRotation = Quaternion.Euler(rotacao, 0, 0);
            transformPlayer.Rotate(Vector3.up, mouseX);
        }
    }
}