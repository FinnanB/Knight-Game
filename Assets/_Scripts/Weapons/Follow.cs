using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        target = transform.parent;
        transform.SetParent(null);
        //target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 back = target.TransformDirection(Vector3.forward) * 0.5f;
        transform.position = (target.position - back) + (Vector3.up*2f);
        transform.LookAt(target);
        Debug.DrawRay(target.position, back, Color.green);
    }
}
