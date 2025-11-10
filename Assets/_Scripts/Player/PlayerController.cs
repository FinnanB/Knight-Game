using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;
using System.IO;
using TMPro;

public struct PlayerStatus
{
    //public string playerName;
    public float maxHealth;
    public float maxStam;
    public float maxMana;
    public float damage;
    public int maxHeals;
    public int level;
    public int exp;
    public int weapon;
}

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6f;
    public Transform cam;
    public Transform enemy;
    public GameObject freeCam;
    public GameObject lockCam;
    public bool lockOn = false;

    public PlayerStatus playerData;

    public float sphereRad;
    public float maxDis;
    public LayerMask layerMask;
    public GameObject currentHitObject;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float health;
    public float stamina;
    public float mana;

    float healthSize;
    public float healthSizeMax;
    public RectTransform m_RectTransform;
    public RectTransform h_RectTransform;

    float manaSize;
    float manaSizeMax;
    public RectTransform m_RectTransform2;
    public RectTransform h_RectTransform2;

    float stamSize;
    public float stamSizeMax;
    public RectTransform m_RectTransform3;
    public RectTransform h_RectTransform3;

    public Image[] hImage;
    public Color c1, c2, c3;

    public int heals;
    public TMP_Text exp;

    public float stamRegen;
    public bool staminaRegening;

    public float _block;

    public float maxSturdy;
    public float sturdy;
    public Animator c_Animator;
    //int maxHeals;

    public MeshRenderer _meshRenderer;
    MaterialPropertyBlock propertyBlock;

    string filePath;
    const string FILE_NAME = "SaveStatus.json";

    public static PlayerController Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        
        filePath = Application.persistentDataPath;
        playerData = new PlayerStatus();
       // ResetData();
        LoadData();
        propertyBlock = new MaterialPropertyBlock();
        sturdy = maxSturdy;
        Reset();
    }

    public void LoadData()
    {
        if (File.Exists(filePath + "/" + FILE_NAME))
        {
            string loadedJson = File.ReadAllText(filePath + "/" + FILE_NAME);
            playerData = JsonUtility.FromJson<PlayerStatus>(loadedJson);
        }
        else
        {
            return;
            //ResetStatus();
        }
    }

    public void ResetData()
    {
        playerData.maxHeals = 3;
        playerData.maxMana = 10;
        playerData.maxHealth = 10;
        playerData.maxStam = 10;
        playerData.damage = 5;
        playerData.weapon = 0;
        playerData.level = 0;
        playerData.exp = 0;
        SaveData();
    }

    public void SaveData()
    {
        //gameStatus.startPos = transform.position;
        //StatisticalData.statInstance.SetStatus(playerData.currentLevel);
        string gameStatusJson = JsonUtility.ToJson(playerData);
        File.WriteAllText(filePath + "/" + FILE_NAME, gameStatusJson);
    }

    public void Reset()
    {
        health = playerData.maxHealth;
        stamina = playerData.maxStam;
        mana = playerData.maxMana;
        heals = playerData.maxHeals;
    }

    public void LevelUp()
    {
        if(playerData.exp >= 10)
        {
            playerData.level++;
            playerData.maxHealth += playerData.maxHealth * 0.2f;
            playerData.damage += playerData.damage * 0.5f;
            playerData.exp -= 10;
        }
        Reset();
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
        if (mana == playerData.maxMana)
        {
            heals = playerData.maxHeals;
            
            mana = 0;
        }
        speed = 6;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        LockOn();
        _Health();
        _UI();
        if (staminaRegening)
        {
            _Stamima();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && mana > 0)
        {
            StopCoroutine(Shield());
            StartCoroutine(Shield());
        }

        propertyBlock.SetFloat("_Alpha", _block);
        _meshRenderer.SetPropertyBlock(propertyBlock);

        if (health <= 0)
        {
            //Destroy(gameObject);
        }
    }

    void _Stamima()
    {
        if (stamina < 0)
        {
            stamina = 0;
            stamRegen = 0.05f;
        }
        else if (stamina < playerData.maxStam)
        {
            stamina += stamRegen;
        }
        else if (stamina >= playerData.maxStam)
        {
            stamina = playerData.maxStam;
            stamRegen = 0.1f;
        }
    }

    void _UI()
    {
        healthSizeMax = playerData.maxHealth *20;
        m_RectTransform.sizeDelta = new Vector2(healthSizeMax, m_RectTransform.sizeDelta.y);
        
        float healthPerc = (playerData.maxHealth -health) / playerData.maxHealth;
        healthSize = (healthSizeMax * healthPerc) +5;
        h_RectTransform.offsetMax = new Vector2(-healthSize, -h_RectTransform.offsetMin.y);

        stamSizeMax = playerData.maxStam * 20;
        m_RectTransform3.sizeDelta = new Vector2(stamSizeMax, m_RectTransform3.sizeDelta.y);

        float stamPerc = (playerData.maxStam - stamina) / playerData.maxStam;
        stamSize = (stamSizeMax * stamPerc) + 5;
        h_RectTransform3.offsetMax = new Vector2(-stamSize, -h_RectTransform3.offsetMin.y);

        manaSizeMax = playerData.maxMana * 20;
        m_RectTransform2.sizeDelta = new Vector2(manaSizeMax, m_RectTransform2.sizeDelta.y);
        float manaPerc = (playerData.maxMana -mana)/ playerData.maxMana;
        manaSize = (manaSizeMax * manaPerc) + 5;
        h_RectTransform2.offsetMax = new Vector2(-manaSize, -h_RectTransform2.offsetMin.y);
        exp.text = new String("EXP: " + playerData.exp);
        for (int i = 0; i < hImage.Length; i++)
        {
            if(i< heals)
            {
                
                hImage[i].color = c1;
            }
            else
            {
                hImage[i].color = c2;
            }
        }
    }

    void _Health()
    {
        if(Input.GetKeyDown("e"))
        {
            StartCoroutine(Heal());
        }
        if (Input.GetKeyDown("f"))
        {
            StartCoroutine(MtoH());
        }
        heals = Mathf.Min(playerData.maxHeals, heals);
        health = Mathf.Min(playerData.maxHealth, health);
        mana = Mathf.Min(playerData.maxMana, mana);
    }

    void Move()
    {
        //bool dodge = false;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);

       /* if (Input.GetKeyDown(KeyCode.Space))
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
    }

    IEnumerator Shield()
    {
        yield return new WaitForSeconds(0.3f);
        _block = 0;
        mana--;
        yield return new WaitForSeconds(0.5f);
        while (mana > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            _block += 0.2f;
            mana--;
            yield return new WaitForSeconds(0.5f);
        }
        _block = 1;
    }

    void LockOn()
    {
        
        RaycastHit hit;
        
        if (Physics.SphereCast(transform.position, sphereRad, transform.forward, out hit, maxDis, layerMask, QueryTriggerInteraction.Ignore))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hit.distance, Color.yellow);
            currentHitObject = hit.transform.gameObject;
            if (currentHitObject.CompareTag("Enemy"))
            {
                enemy = currentHitObject.transform;
            }
        }
        else
        {
            currentHitObject = null;
            enemy = null;
        }
        if (lockOn && enemy != null)
        {
            lockCam.GetComponent<CinemachineFreeLook>().LookAt = enemy;
            lockCam.SetActive(true);

            freeCam.SetActive(false);
        }
        else if (!lockOn || enemy == null)
        {
            lockCam.SetActive(false);
            freeCam.SetActive(true);
            lockOn = false;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!lockOn && enemy != null)
            {
                lockOn = true;
            }
            else if (lockOn)
            {
                lockOn = false;
                enemy = null;
            }
        }
    }

    public void Hit(float dam)
    {
        StopCoroutine(_Fall());
        health -= dam*_block;
        sturdy--;
        StartCoroutine(_Fall());
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
            c_Animator.SetTrigger("Fall");
            sturdy = maxSturdy;
        }
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void Sprint()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = 12;
        }
        else
        {
            speed = 6;
        }
    }
}
