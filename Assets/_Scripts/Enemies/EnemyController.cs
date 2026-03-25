using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

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

    public float armor;

    public bool seen;

    bool hasDied;

    public int xp;

    public Vector3 startPos;
    Vector3 startEulerAngles;
    public float rotateSpeed = 1.0f;

    float sturdyTime;
    public float sturdyResetTime;

    public void Hit(float dam, GameObject other, bool damageType, float pierce)
    {
        targetObject = other.transform;
        seen = true;
        StopCoroutine(_Fall());
        float damageTaken;
        if (damageType)
        {
            damageTaken = Mathf.Max(1, dam-(armor*(10-pierce)/10));
            
        }
        else
        {
            damageTaken = Mathf.Max(0, dam - armor);
        }
        Debug.Log(damageTaken);
        if (damageTaken == 0)
        {
            other.GetComponent<Animator>().SetTrigger("HitWrong");
        }
        health -= damageTaken;
        float sturDam = damageTaken / 3;
        if (sturDam > 0)
        {
            sturdyTime = 0;
        }
        sturdy += sturDam;
        StartCoroutine(_Fall());
    }

    public void Reset()
    {
        hasDied = false;
        c_Animator.SetTrigger("Reset");
        health = maxHealth;
        transform.position = startPos;
        transform.eulerAngles = startEulerAngles;
        seen = false;
        isClose = false;
        // Debug.Log(seen);
        destination = transform.position; 
        //Debug.Log(destination);
    }

    IEnumerator _Fall()
    {
        yield return new WaitForSeconds(1.5f);
        //sturdy = maxSturdy;
    }

    void Sturdy()
    {
        if (sturdy > 0)
        {
            sturdyTime += Time.deltaTime;
        }
        else if (sturdy == 0)
        {
            sturdyTime = 0;
        }

        if (sturdyTime >= sturdyResetTime)
        {
            sturdy = 0;
        }
        if (sturdy >= maxSturdy && health > 0)
        {
           // Debug.Log(sturdy);
            c_Animator.SetTrigger("Fall");
            StartCoroutine(_Fall());
            sturdy = 0;
        }
    }

    void Start()
    {
        sturdyTime = 0;
        startPos = transform.position;
        startEulerAngles = transform.eulerAngles;
        seen = false;
        navAgent = GetComponent<NavMeshAgent>();
        sturdy = 0;
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
            FacePlayer();
        }
        else
        {
            destination = transform.position;
        }
        canSee();
        Sturdy();
        if(health <= 0 && !hasDied)
        {
            StartCoroutine(_Death());
        }
        
        navAgent.SetDestination(destination);
    }

    void FacePlayer()
    {
        Vector3 targetDirection = targetObject.position - transform.position;
        float singleStep = rotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        if (navAgent.stoppingDistance >= navAgent.remainingDistance)
        {
            transform.rotation = Quaternion.LookRotation(newDirection);
            //Debug.Log(targetDirection.normalized);
            Debug.DrawRay(transform.position, targetDirection, Color.green);
            navAgent.Move(-targetDirection *0.5f* Time.deltaTime);
        }
    }

    IEnumerator _Death()
    {
        hasDied = true;
        targetObject.GetComponent<PlayerController>().playerData.exp += xp;
        c_Animator.SetBool("Died", true);
        yield return new WaitForEndOfFrame();
        c_Animator.SetBool("Died", false);
        yield return new WaitForSeconds(1.25f);
        gameObject.SetActive(false);
    }
}
