using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public float maxSturdy;
    public float sturdy;
    public Animator c_Animator;

    public Vector3 destination;
    NavMeshAgent navAgent;

    public bool isVisible;
    int layerMask = 1 << 3;
    public bool isClose;

    public Transform targetObject;
    public Transform eyes;

    public bool seen;

    public int xp;

    public Vector3 startPos;
    Vector3 startEulerAngles;



    public void Hit(float dam, GameObject other)
    {
        targetObject = other.transform;
        seen = true;
        StopCoroutine(_Fall());
        health -= dam;
        sturdy--;
        StartCoroutine(_Fall());
    }

    public void Reset()
    {
        health = maxHealth;
        transform.position = startPos;
        transform.eulerAngles = startEulerAngles;
        seen = false;
       // Debug.Log(seen);
        destination = transform.position;
    }

    IEnumerator _Fall()
    {
        yield return new WaitForSeconds(10f);
        sturdy = maxSturdy;
    }

    void Sturdy()
    {
        if (sturdy <= 0)
        {
            
            sturdy = maxSturdy;
        }
        if (health <= 0)
        {
            targetObject.GetComponent<PlayerController>().playerData.exp += xp; 
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        startPos = transform.position;
        startEulerAngles = transform.eulerAngles;
        seen = false;
        navAgent = GetComponent<NavMeshAgent>();
        sturdy = maxSturdy;
        destination = transform.position;
        targetObject = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void canSee()
    {
        RaycastHit hit;
        if (Physics.Raycast(eyes.position, transform.TransformDirection(Vector3.forward), out hit, 100, ~layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(eyes.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log(hit.transform.gameObject.layer);
            isPlayerVisible(hit.distance);
        }
        else
        {
            Debug.DrawRay(eyes.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            isPlayerVisible(100);
        }
    }

    void isPlayerVisible(float dis)
    {
        RaycastHit hit2;
        if (Physics.SphereCast(eyes.position, 1, transform.TransformDirection(Vector3.forward), out hit2, dis, layerMask))
        {
            isVisible = true;
        }
        else
        {
            isVisible = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        c_Animator.SetFloat("Sturdy", sturdy);
        float dis = Vector3.Distance(targetObject.position, transform.position);
        if (dis <= 50f)
        {
            isClose = true;
        }
        else
        {
            isClose = false;
        }
        
        if (isVisible && isClose)
        {
            seen = true;
        }
        else if(!isClose)
        {
            seen = false;
            destination = transform.position;
        }
        if(seen)
        {
            destination = targetObject.position;
        }
        else
        {
            destination = transform.position;
        }
        canSee();
        Sturdy();
        if(health <= 0)
        {
            gameObject.SetActive(false);
        }
        navAgent.SetDestination(destination);
    }
}
