using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Inimigo
{
    public class RegDoll : MonoBehaviour
    {
        List<Rigidbody> regDollRigids = new List<Rigidbody>();
        public Rigidbody rigid;
        List<Collider> regdollCollider = new List<Collider>();
        void Start()
        {
            rigid = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void AtivaRegdoll()
        {
            for (int i = 0; i < regDollRigids.Count; i++)
            {
                regDollRigids[i].isKinematic = false;
                regdollCollider[i].isTrigger = false;
                regDollRigids[i].transform.gameObject.layer = 10;
            }
            rigid.isKinematic = true;
            GetComponent<CapsuleCollider>().isTrigger = true;
            StartCoroutine(nameof(FinalizaAnimacao));
        }
        IEnumerator FinalizaAnimacao()
        {
            yield return new WaitForEndOfFrame();
            GetComponent<Animator>().enabled = false;
            this.enabled = false;

        }
        public void DesativarRegdoll()
        {
            Rigidbody[] rigs = GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < rigs.Length; i++)
            {
                if (rigs[i] == rigid)
                {
                    continue;
                }
                regDollRigids.Add(rigs[i]);
                rigs[i].isKinematic = true;

                Collider col = rigs[i].gameObject.GetComponent<Collider>();
                col.isTrigger = true;
                regdollCollider.Add(col);
            }
        }
    }
}