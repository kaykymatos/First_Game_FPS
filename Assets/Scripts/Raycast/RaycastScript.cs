using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Raycast
{
    public class RaycastScript : MonoBehaviour
    {
        public float distanciaAlvo;
        public GameObject objArrastar, objPegar;
        RaycastHit hit;

        void Update()
        {
            if (Time.frameCount % 5 == 0)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward)*5,Color.red);
                if(Physics.SphereCast(transform.position,0.1f, transform.TransformDirection(Vector3.forward), out hit))
                {
                    distanciaAlvo = hit.distance;
                    if (hit.transform.gameObject.tag == "objArrasta")
                    {
                        objArrastar = hit.transform.gameObject;
                    }
                    else
                    {
                        objArrastar = null;
                    }
                    if (hit.transform.gameObject.tag == "objPegar")
                    {
                        objPegar = hit.transform.gameObject;
                    }
                    else
                    {
                        objPegar = null;
                    }
                }
                
            }
        }
    }
}
