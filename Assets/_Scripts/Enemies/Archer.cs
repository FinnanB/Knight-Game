using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Archer : MonoBehaviour
{

    public bool launch;
    public GameObject prefab;
    public float push;
    GameObject arrow;
    public bool knocked;

    // Start is called before the first frame update
    void Start()
    {
        launch = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(knocked)
        {
            Destroy(arrow);
            StopAllCoroutines();
            launch = true;
        }
        bool seen = GetComponent<EnemyController>().isVisible;
        bool inRange = GetComponent<EnemyController>().isClose;
        if (launch && seen && inRange)
        {
            StartCoroutine(Launch());
        }
    }
    
    IEnumerator Launch()
    {
        launch = false;
        GetComponent<NavMeshAgent>().angularSpeed = 40;
        //launch = false;
        Vector3 spawn = transform.position - (transform.right * 0.6f);//new Vector3(transform.localPosition.x - 0.5f, transform.localPosition.y, transform.localPosition.z);
        arrow = Instantiate(prefab, spawn, transform.rotation, transform);
        yield return new WaitForSeconds(1f);
        GetComponent<NavMeshAgent>().angularSpeed = 0;
        yield return new WaitForSeconds(0.5f);
        Destroy(arrow);
        spawn = transform.position + (transform.forward * 2);//new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.5f);
        arrow = Instantiate(prefab, spawn, transform.rotation, transform);
        yield return new WaitForSeconds(0.01f);
        yield return new WaitForSeconds(0.01f);
        arrow.transform.SetParent(null);
        arrow.GetComponent<Collider>().enabled = true;
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddRelativeForce(Vector3.forward * push * (1.5f));
        GetComponent<NavMeshAgent>().angularSpeed = 180;
        yield return new WaitForSeconds(2f);
        launch = true;
    }
}
