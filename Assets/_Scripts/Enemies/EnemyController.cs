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
    public bool isBoss;

    public GameObject menu;
    public GameObject pause;
    public GameObject win;

    int layerIndex;

    public void Hit(float dam, GameObject other, bool damageType, float pierce, Vector3 dir)
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
        //Debug.Log(damageTaken);
        if (damageTaken == 0)
        {
            other.GetComponent<Animator>().SetTrigger("HitWrong");
        }
        health -= damageTaken;
        float sturDam = damageTaken / 5;
        if (sturDam > 0)
        {
            sturdyTime = 0;
        }
        sturdy += sturDam;
        if ((sturDam >= maxSturdy / 3 && sturdy < maxSturdy) && (health > 0))
        {
            c_Animator.SetTrigger("Stumble");
            StartCoroutine(_Stumble(dir));
        }
       // Debug.Log(damageTaken);
        Debug.Log(sturDam);
        
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

    IEnumerator _Stumble(Vector3 dir)
    {
        Debug.Log("a " + this);
        canMove = false;
        float a = 0;
        Vector3 moveDir = dir - transform.position;
        GetComponent<Skelly>().PAnim();
        //Debug.Log(moveDir);
        while (a < 0.6f)
        {
            float step = Time.deltaTime * 1f;

            navAgent.Move(moveDir * -step);
            a += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log("t" + Time.time);
        canMove = true;
        yield return null;
    }

    IEnumerator _Fall()
    {
        c_Animator.SetLayerWeight(layerIndex, 0);
        //Debug.Log("f " + Time.time + " " + (1.8f + Time.time));
        canMove = false;
        GetComponent<Skelly>().PAnim();
        yield return new WaitForSeconds(1.8f);
        //Debug.Log("y" + Time.time);
        c_Animator.SetLayerWeight(layerIndex, 1);
        canMove = true;
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
            StopAllCoroutines();
            StartCoroutine(_Fall());
            sturdy = 0;
        }
    }

    void Start()
    {
        c_Animator.SetBool("IsBoss", isBoss);
        layerIndex = c_Animator.GetLayerIndex("Move");
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

    public bool check;
    // Update is called once per frame

    string m_ClipName;
    AnimatorClipInfo[] m_CurrentClipInfo;
    void Update()
    {
        
        // Debug.Log(this + " b " + m_CurrentClipInfo[0].clip.name);

        if (check != canMove)
        {
          //  Debug.Log("test " + Time.time);
            check = canMove;
        }

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
            if(canMove && !hasDied)
            {
                c_Animator.SetLayerWeight(layerIndex, 1);
                //Debug.Log("h" + Time.time);
                FacePlayer();
            }
            else
            {
                c_Animator.SetFloat("Move", 0);
                c_Animator.SetLayerWeight(layerIndex, 0);
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

    public bool a;

    void FacePlayer()
    {
        Vector3 targetDirection = new Vector3(targetObject.position.x - transform.position.x,0, targetObject.position.z - transform.position.z);
        tDir = transform.TransformDirection(Vector3.back) * speed * Time.deltaTime; 
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
            a = true;
            navAgent.Move(tDir);
            relative = transform.InverseTransformDirection(tDir);
        }
        else 
        {
            a = false;
            relative = transform.InverseTransformDirection(navAgent.velocity);
        }
        MoveAnim();
    }

    IEnumerator _Death()
    {
        canMove = false;
        c_Animator.SetLayerWeight(layerIndex, 0);
        
        hasDied = true;
        targetObject.GetComponent<PlayerController>().playerData.exp += xp;
       // m_CurrentClipInfo = c_Animator.GetCurrentAnimatorClipInfo(0);
       // Debug.Log(this + " a " + m_CurrentClipInfo[0].clip.name);
        c_Animator.SetBool("Died", true);
        c_Animator.SetTrigger("Die");
        //yield return new WaitForEndOfFrame();
        
        
        yield return new WaitForSeconds(0.15f);
        c_Animator.SetBool("Died", false);
        
        m_CurrentClipInfo = c_Animator.GetCurrentAnimatorClipInfo(0);
        if (m_CurrentClipInfo[0].clip.name != "SkellyDie")
        {
            Debug.Log(this + " b " + m_CurrentClipInfo[0].clip.name);
        }
        yield return new WaitForSeconds(1.25f);
        
        if (isBoss)
        {
            targetObject.GetComponent<PlayerController>().readTime = false;
            menu.SetActive(true);
            pause.SetActive(false);
            win.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            Destroy(gameObject);
        }
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
