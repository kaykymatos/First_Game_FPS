using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Inimigo
{
    public class RegDoll : MonoBehaviour
    {
        List<Rigidbody> regDollRigids = new();
        public Rigidbody rigid;
        List<Collider> regdollCollider = new();
        void Start()
        {
            rigid = GetComponent<Rigidbody>();
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
        public IEnumerator SomeMorto()
        {
            yield return new WaitForSeconds(10);
            rigid.isKinematic = false;
            DesativarRegdoll();
            yield return new WaitForSeconds(4);
            Destroy(this.gameObject);
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