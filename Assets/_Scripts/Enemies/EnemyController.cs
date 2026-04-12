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

    public Vector3 tDir;
    public bool canMove;
    public float speed;

    GameObject resetOb;

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
        /*hasDied = false;
        c_Animator.SetTrigger("Reset");
        health = maxHealth;
        transform.position = startPos;
        transform.eulerAngles = startEulerAngles;
        seen = false;
        isClose = false;
        // Debug.Log(seen);
        destination = transform.position; 
        //Debug.Log(destination);
        transform.eulerAngles = startEulerAngles;
        Instantiate(gameObject, startPos, Quaternion.identity, transform.parent);*/
        //resetOb.SetActive(true);
        Destroy(gameObject);
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
            if(canMove)
            {
                FacePlayer();
            }
            else
            {
                c_Animator.SetFloat("Move", 0);
            }
            
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

    Vector3 relative;

    void MoveAnim()
    {
        //Vector3 relative = transform.InverseTransformDirection(navAgent.velocity);
        relative = relative.normalized;
        //Debug.Log(relative);
        if(relative.z >= 0.1f || relative.z <= -0.1f)
        {
            c_Animator.SetFloat("Move", relative.z);
        }
        else
        {
            c_Animator.SetFloat("Move", 0);
        }
        
    }

    void FacePlayer()
    {
        Vector3 targetDirection = targetObject.position - transform.position;
        tDir = targetDirection.normalized * -5 * Time.deltaTime; ;
        float singleStep = rotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        float angle = Vector3.Angle(targetDirection, transform.forward);
        if (navAgent.stoppingDistance >= navAgent.remainingDistance && angle >= 2)
        {
            transform.rotation = Quaternion.LookRotation(newDirection);
            
            //Debug.Log(angle);
            Debug.DrawRay(transform.position, targetDirection, Color.green);
        }
        if (navAgent.stoppingDistance >= navAgent.remainingDistance + 1 && angle <=25)
        {
            navAgent.Move(tDir);
            relative = transform.InverseTransformDirection(tDir);
        }
        else 
        {
            relative = transform.InverseTransformDirection(navAgent.velocity);
        }
        MoveAnim();
    }

    IEnumerator _Death()
    {
        hasDied = true;
        targetObject.GetComponent<PlayerController>().playerData.exp += xp;
        c_Animator.SetBool("Died", true);
        yield return new WaitForEndOfFrame();
        c_Animator.SetBool("Died", false);
        yield return new WaitForSeconds(1.25f);
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
