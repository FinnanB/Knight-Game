using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform target;
    public GameObject prefab;
    public Vector3 dir;
    List<GameObject> chain = new List<GameObject>();

    void Update()
    {
        transform.LookAt(target, Vector3.up);
        dir = target.position - transform.position;
        dir = dir.normalized;
        int dist = (int)Vector3.Distance(target.position, transform.position);
        if(dist != chain.Count)
        {
            FillChain(dist);
        }
        
    }

    void FillChain(int dist)
    {
        if (chain.Count < dist)
        {
            for (int i = chain.Count; i < dist; i++)
            {
                Vector3 spawn = transform.localPosition + (dir *i);
                GameObject pre = Instantiate(prefab, spawn, transform.rotation, transform);
                chain.Add(pre);
            }
        }
        if (chain.Count > dist)
        {
            for (int i = chain.Count; i > dist; i--)
            {
                Destroy(chain[chain.Count - 1]);
                chain.RemoveAt(chain.Count - 1);
            }
        }
    }
}
