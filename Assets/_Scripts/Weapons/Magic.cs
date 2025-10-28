using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public bool launch;

    public float speed;

    void Start()
    {
        launch = false;
    }

    void Update()
    {
        if(launch)
        {
            transform.SetParent(null);
            GetComponent<Collider>().enabled = true;
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
