using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    public List<Rope> ropes;
    public TMPro.TextMeshProUGUI lineText;

    private void FixedUpdate()
    {
        if (ropes != null)
        {
            NativeArray<JobHandle> allHandles = new NativeArray<JobHandle>(ropes.Count, Allocator.Temp);
            for (int i = 0; i < ropes.Count; i++)
            {
                allHandles[i] = ropes[i].SimulateBurst();
            }
            JobHandle.CompleteAll(allHandles);
        }
    }
    private void Update()
    {
        if (ropes != null)
        {
            for (int i = 0; i < ropes.Count; i++)
            {
                ropes[i].RopeUpdate();
            }
        }
    }

}
