using System.IO;
using UnityEngine;

namespace Scripts.CriaIcone
{
    [ExecuteInEditMode]
    public class CriaIcone : MonoBehaviour
    {
        public bool criar;
        public RenderTexture ren;
        public Camera bakeCamera;
        public string spriteName;
        void Update()
        {
            if (criar)
            {
                criar = false;
                CriaIconeAgora();
            }
        }
        void CriaIconeAgora()
        {
            if (string.IsNullOrEmpty(spriteName))
            {
                spriteName = "icone";
            }
            string path = Savelocal();
            path += spriteName;
            bakeCamera.targetTexture = ren;
            RenderTexture texturaAtual = RenderTexture.active;
            bakeCamera.targetTexture.Release();
            RenderTexture.active = bakeCamera.targetTexture;
            bakeCamera.Render();

            Texture2D imgPng = new Texture2D(bakeCamera.targetTexture.width, bakeCamera.targetTexture.height, TextureFormat.ARGB32, false);
            imgPng.ReadPixels(new Rect(0, 0, bakeCamera.targetTexture.width, bakeCamera.targetTexture.height), 0, 0);
            imgPng.Apply();

            RenderTexture.active = texturaAtual;
            byte[] bytesPng = imgPng.EncodeToPNG();
            File.WriteAllBytes(path + ".png", bytesPng);
            Debug.Log(spriteName + " Criado!");
        }
        string Savelocal()
        {
            string saveLocal = Application.streamingAssetsPath + "/Icones/";
            if (!Directory.Exists(saveLocal))
            {
                Directory.CreateDirectory(saveLocal);
            }
            return saveLocal;
        }
    }
}