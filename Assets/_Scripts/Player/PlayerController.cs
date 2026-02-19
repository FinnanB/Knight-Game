using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;
using System.IO;
using TMPro;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.Rendering;

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
    public Vector3 spawnPoint;
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

    public Vector3 spawnPoint;

    string filePath;
    const string FILE_NAME = "SaveStatus.json";

    public bool canMove;

    public bool lockMouse;
    public Collider hitZone;

    public float dodgeCost;

    public bool canSprint;
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
        if(lockMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        filePath = Application.persistentDataPath;
        playerData = new PlayerStatus();
        // ResetData();
        
        LoadData();
        //playerData.spawnPoint = spawnPoint;
        propertyBlock = new MaterialPropertyBlock();
        sturdy = maxSturdy;
        Reset();
    }

    public void SetPosition()
    {

        playerData.spawnPoint = transform.position;
        SaveData();
        //Debug.Log(playerData.spawnPoint);
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
        playerData.maxMana = 150;
        playerData.maxHealth = 200;
        playerData.maxStam = 200;
        playerData.damage = 5;
        playerData.weapon = 0;
        playerData.level = 0;
        playerData.exp = 0;
        transform.position = Vector3.zero;
        playerData.spawnPoint = transform.position;
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
        transform.position = playerData.spawnPoint;
    }

    public void LevelUp()
    {
        if(playerData.exp >= (10*Mathf.Pow(1.5f, playerData.level)))
        {
            playerData.level++;
            Debug.Log(10 + (Mathf.Pow(1.1f, playerData.level)));
            playerData.maxHealth += playerData.maxHealth * 0.2f;
            playerData.damage += playerData.damage * 0.5f;
            playerData.exp -= 10;
        }
        Reset();
    }

    IEnumerator Heal()
    {
        canSprint = false;
        speed = 3;
        yield return new WaitForSeconds(0.3f);
        if (heals > 0)
        {
            health += 30;
            
            heals--;
        }
        speed = 6;
        canSprint = true;
    }

    IEnumerator MtoH()
    {
        canSprint = false;
        speed = 1;
        yield return new WaitForSeconds(3f);
        if (mana >= 120)
        {
            heals = playerData.maxHeals;
            
            mana -= 120;
        }
        speed = 6;
        canSprint = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (mana < 0)
        {
            mana = 0;
        }
        //speed = 12;
        if (canMove)
        {
            Move();
            if (canSprint)
            {
                Sprint();
            }
        }

        
        LockOn();
        _Health();
        
        _UI();
        if (staminaRegening)
        {
            _Stamima();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && mana > 0)
        {
            StopCoroutine(Shield());
            StartCoroutine(Shield());
        }

        propertyBlock.SetFloat("_Alpha", _block);
        _meshRenderer.SetPropertyBlock(propertyBlock);

        if (health <= 0)
        {
            playerData.exp = 0;
            SaveData();
            Destroy(gameObject);
        }
    }

    void _Stamima()
    {
        if (stamina < 0)
        {
            stamina = 0;
            stamRegen = 5f;
        }
        else if (stamina < playerData.maxStam)
        {
            stamina += stamRegen*Time.deltaTime;
        }
        else if (stamina >= playerData.maxStam)
        {
            stamina = playerData.maxStam;
            stamRegen = 10f;
        }
    }

    void _UI()
    {
        healthSizeMax = playerData.maxHealth;
        m_RectTransform.sizeDelta = new Vector2(healthSizeMax, m_RectTransform.sizeDelta.y);
        
        float healthPerc = (playerData.maxHealth -health) / playerData.maxHealth;
        healthSize = (healthSizeMax * healthPerc) +5;
        h_RectTransform.offsetMax = new Vector2(-healthSize, -h_RectTransform.offsetMin.y);

        stamSizeMax = playerData.maxStam;
        m_RectTransform3.sizeDelta = new Vector2(stamSizeMax, m_RectTransform3.sizeDelta.y);

        float stamPerc = (playerData.maxStam - stamina) / playerData.maxStam;
        stamSize = (stamSizeMax * stamPerc) + 5;
        h_RectTransform3.offsetMax = new Vector2(-stamSize, -h_RectTransform3.offsetMin.y);

        manaSizeMax = playerData.maxMana;
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

        c_Animator.SetFloat("Sides", Mathf.Clamp(horizontal * speed, -1f, 1f));
        c_Animator.SetFloat("Forward", Mathf.Clamp(vertical *speed, -1f, 1f));

        transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);

        if (Input.GetKeyDown(KeyCode.Space) && stamina >= dodgeCost)
        {
            StartCoroutine(_Dodge(direction));
            //dodge = true;
        }
        if (direction.magnitude >= 0.1f)
        {
            staminaRegening = false;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * (speed + direction.z) * Time.deltaTime);
        }
        else
        {
            staminaRegening = true;
        }
    }

    IEnumerator _Dodge(Vector3 dir)
    {
        stamina -= dodgeCost;
        staminaRegening = false;
        canMove = false;
        if (dir.magnitude == 0)
        {
            dir.z = 1f;
        }
        c_Animator.SetFloat("Forward", dir.z);
        c_Animator.SetFloat("Sides", dir.x);
        c_Animator.SetTrigger("Roll");
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        Vector3 target = transform.position + dir*10;
        Debug.Log(target);
        float a = 0;
        while (a < 1f)
        {
            float step = Time.deltaTime * 10;
            controller.Move(moveDir*step);
            a += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        canMove = true;
        staminaRegening = true;
        yield return null;
    }

    IEnumerator Shield()
    {
        yield return new WaitForSeconds(0.3f);
        _block = 0;
        mana -= 30;
        yield return new WaitForSeconds(0.5f);
        while (mana > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            _block += 0.2f;
            mana -= 30;
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
        //hitZone.enabled = false;

        yield return new WaitForSeconds(0.75f);
        //hitZone.enabled = true;
       // yield return new WaitForSeconds(1f);
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
        if(Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            
            stamina -= 30*Time.deltaTime;
            staminaRegening = false;
            speed = 8;
        }
        else
        {
            staminaRegening = true;
            speed = 6;
        }
    }
}
