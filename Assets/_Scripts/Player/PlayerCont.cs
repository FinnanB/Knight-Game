using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;
using System.IO;
using TMPro;

public class PlayerCont : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public float maxStam;
    public float stam;
    public float maxMana;
    public float mana;
    public int maxHeals;
    public int heals;
    public int level;
    public int exp;

    public CharacterController controller;
    public float speed = 6f;
    public Transform cam;
    public Transform enemy;
    public GameObject freeCam;
    public GameObject lockCam;
    public bool lockOn = false;

    public float sphereRad;
    public float maxDis;
    public LayerMask layerMask;
    public GameObject currentHitObject;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        //bool dodge = false;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y + 90f, 0f);

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            c_Animator.SetFloat("Forward", horizontal);
            c_Animator.SetFloat("Sides", vertical);
            c_Animator.SetTrigger("Dodge");
            //dodge = true;
        }*/
        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        _Health();
    }

    void _Health()
    {
        if (Input.GetKeyDown("e"))
        {
            StartCoroutine(Heal());
        }
        if (Input.GetKeyDown("f"))
        {
            StartCoroutine(MtoH());
        }
        heals = Mathf.Min(maxHeals, heals);
        health = Mathf.Min(maxHealth, health);
        mana = Mathf.Min(maxMana, mana);
    }

    IEnumerator Heal()
    {
        speed = 3;
        yield return new WaitForSeconds(0.3f);
        if (heals > 0)
        {
            health += 3;

            heals--;
        }
        speed = 6;
    }

    IEnumerator MtoH()
    {
        speed = 1;
        yield return new WaitForSeconds(3f);
        if (mana == maxMana)
        {
            heals = maxHeals;

            mana = 0;
        }
        speed = 6;
    }

    /*void _UI()
    {
        healthSizeMax = playerData.maxHealth * 20;
        m_RectTransform.sizeDelta = new Vector2(healthSizeMax, m_RectTransform.sizeDelta.y);

        float healthPerc = (playerData.maxHealth - health) / playerData.maxHealth;
        healthSize = (healthSizeMax * healthPerc) + 5;
        h_RectTransform.offsetMax = new Vector2(-healthSize, -h_RectTransform.offsetMin.y);

        manaSizeMax = playerData.maxMana * 20;
        m_RectTransform2.sizeDelta = new Vector2(manaSizeMax, m_RectTransform.sizeDelta.y);
        float manaPerc = (playerData.maxMana - mana) / playerData.maxMana;
        manaSize = (manaSizeMax * manaPerc) + 5;
        h_RectTransform2.offsetMax = new Vector2(-manaSize, -h_RectTransform2.offsetMin.y);
        exp.text = new String("EXP: " + playerData.exp);
        for (int i = 0; i < hImage.Length; i++)
        {
            if (i < heals)
            {

                hImage[i].color = c1;
            }
            else
            {
                hImage[i].color = c2;
            }
        }
    }*/
}
