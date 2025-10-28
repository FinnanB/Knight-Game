using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnakeController : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public GameObject mainSnake;
    public SnakeController head;
    public float maxSturdy;
    public float sturdy;

    public NavMeshAgent agent;

    Vector3 startPos;
    Vector3 startEulerAngles;

    float healthSize;
    public float healthSizeMax;
    public RectTransform m_RectTransform;
    public RectTransform h_RectTransform;
    // Start is called before the first frame update
    void Start()
    {
        mainSnake = transform.root.gameObject;
        head = mainSnake.GetComponent<SnakeController>();
        agent = GetComponentInChildren<NavMeshAgent>();
        health = maxHealth;
        sturdy = maxSturdy;
        startPos = transform.position;
        startEulerAngles = transform.eulerAngles;
        Reset();
    }

    public void Hit(float dam)
    {
        head.StopCoroutine(_Fall());
        head.health -= dam;
        head.sturdy--;
        head.StartCoroutine(_Fall());
    }

    void Sturdy()
    {
        if (sturdy <= 0)
        {
            StartCoroutine(Fallen());
            sturdy = maxSturdy;
        }
        if (health <= 0)
        {
            //targetObject.GetComponent<PlayerController>().playerData.exp += xp;
            gameObject.SetActive(false);
        }
    }

    IEnumerator Fallen()
    {
        agent.enabled = false;
        yield return new WaitForSeconds(3f);
        agent.enabled = true;
    }

    IEnumerator _Fall()
    {
        yield return new WaitForSeconds(10f);
        sturdy = maxSturdy;
    }

    public void Reset()
    {
        health = maxHealth;
        transform.position = startPos;
        transform.eulerAngles = startEulerAngles;
        agent.enabled = false;
    }

    void UI()
    {
        if(m_RectTransform != null)
        {
            healthSizeMax = maxHealth;
            m_RectTransform.sizeDelta = new Vector2(healthSizeMax, m_RectTransform.sizeDelta.y);

            float healthPerc = (maxHealth - health) / maxHealth;
            healthSize = (healthSizeMax * healthPerc) + 5;
            h_RectTransform.offsetMax = new Vector2(-healthSize, -h_RectTransform.offsetMin.y);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        UI();
        Sturdy();
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
