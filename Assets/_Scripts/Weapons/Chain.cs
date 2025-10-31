using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public Transform target;
    public GameObject prefab;
    public GameObject hitBox;
    Vector3 dir;
    List<GameObject> chain = new List<GameObject>();
    int length;

    void Update()
    {
        transform.LookAt(target, Vector3.up);
        dir = target.position - transform.position;
        dir = dir.normalized;
        float dist = Vector3.Distance(target.position, transform.position);
        length = (int)(dist / 0.15f);
        if (dist != chain.Count)
        {
            FillChain(length);
        }
        ChainHitbox();
    }

    void FillChain(int dist)
    {
        if (chain.Count < dist)
        {
            for (int i = chain.Count; i < dist; i++)
            {
                Vector3 spawn = transform.position + (dir * i * 0.15f);
                GameObject pre = Instantiate(prefab, spawn, transform.rotation, transform);
                pre.transform.localRotation = Quaternion.Euler(0, 0, 90 * i);
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

    void ChainHitbox()
    {
        CapsuleCollider hit = hitBox.GetComponent<CapsuleCollider>();
        if (chain.Count != 0)
        {
            hit.enabled = true;
            Vector3 newPos = chain[chain.Count/2].transform.position;
            hitBox.transform.position = newPos;
            hit.height = chain.Count * 0.15f;
        }
        else if(chain.Count == 0)
        {
            hit.enabled = false;
        }
    }
}
