using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;
using UnityEditor;

public struct SwordStatus
{
    public int level;
    public bool[] unlocked;
    public bool[] selected;
}

public class Sword : MonoBehaviour
{
    public SwordStatus swordData;

    public Animator c_Animator;

    public bool swing;
    public bool wMode;

    public float swingCost;

    public bool changed;

    public List<string> _Attacks = new List<string>();
    public int[] ints = new int[2];
    public Toggle[] _Tog = new Toggle[2];
    public int currentMoves;
    public int totalMoves;

    public bool[] _unlocked;

    public Collider sw1;
    public Collider sw2;
    public Collider sw3;

    public int lvl;

    public bool run;
    public TMP_Text moveCount;

    public GameObject modeImage1, modeImage2;

    string filePath;
    const string FILE_NAME = "WeaponStatus.json";

    public static Sword sInstance { get; private set; }

    void Awake()
    {
        if (sInstance != null && sInstance != this)
        {
            Destroy(this);
        }
        else
        {
            sInstance = this;
        }
    }

    public void LevelUp()
    {
        swordData.level = 2;
        SaveData();
    }


    void Start()
    {
        swing = true;
        wMode = false;
        filePath = Application.persistentDataPath;
        swordData = new SwordStatus();
        foreach (AnimatorControllerParameter parameter in c_Animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                _Attacks.Add(parameter.name);
            }
        }
        //ResetData();
        LoadData();
        _unlocked = swordData.unlocked;
    }

    public void ResetData()
    {
        swordData.level = 1;
        swordData.unlocked = new bool[2];
        swordData.selected = new bool[2];
        for (int i = 0; i < swordData.unlocked.Length; i++)
        {
            swordData.unlocked[i] = false;
            swordData.selected[i] = false;
        }
        SaveData();
        Debug.Log("b " + swordData.unlocked[0]);
        LoadData();
        Debug.Log("c " + swordData.unlocked[0]);
    }

    public void LoadData()
    {
        if (File.Exists(filePath + "/" + FILE_NAME))
        {
            string loadedJson = File.ReadAllText(filePath + "/" + FILE_NAME);
            swordData = JsonUtility.FromJson<SwordStatus>(loadedJson);
        }
        else
        {
            return;
            //ResetStatus();
        }
        Debug.Log("d " + swordData.unlocked[0]);
        for (int i = 0; i < _Tog.Length; i++)
        {
            Debug.Log("f " + i + " " + swordData.unlocked[0]);
            _Tog[i].isOn = swordData.selected[i];
            _Tog[i].gameObject.SetActive(swordData.unlocked[i]);
            Debug.Log("g " + i + " " + swordData.unlocked[0]);
        }
        Debug.Log("e " + swordData.unlocked[0]);
    }

    public void SetData(int a)
    {
        //Debug.Log(a);
       // Debug.Log("d " + swordData.unlocked[0]);
        for (int i = 0; i < _Tog.Length; i++)
        {
            swordData.selected[i] = _Tog[i].isOn;
            swordData.unlocked[i] = _Tog[i].gameObject.activeSelf;
        }
      //  Debug.Log("e " + swordData.unlocked[0]);
        SaveData();
    }

    public void SaveData()
    {
        //gameStatus.startPos = transform.position;
        //StatisticalData.statInstance.SetStatus(playerData.currentLevel);
        string gameStatusJson = JsonUtility.ToJson(swordData);
        File.WriteAllText(filePath + "/" + FILE_NAME, gameStatusJson);
    }



    public void Update()
    {
        
        lvl = swordData.level;
        _unlocked = swordData.selected;
        modeImage1.SetActive(wMode);
        modeImage2.SetActive(!wMode);
        c_Animator.SetBool("Switch", wMode);
        totalMoves = swordData.level;
        moveCount.text = "Attacks: " + currentMoves + "/" + totalMoves;

        if (Input.GetMouseButtonDown(0) && GetComponent<PlayerController>().stamina >= swingCost && swing)
        {
            GetComponent<PlayerController>().stamina -= swingCost;
            if(wMode & GetComponent<PlayerController>().mana >= swingCost)
            {
                GetComponent<PlayerController>().mana -= swingCost;
            }
            else if(wMode & GetComponent<PlayerController>().mana <= swingCost)
            {
                return;
            }
            c_Animator.SetTrigger("Swing");
            StartCoroutine(PlayAnimation());
        }
        if (Input.GetMouseButtonDown(1) && GetComponent<PlayerController>().stamina >= swingCost*2 && GetComponent<PlayerController>().mana >= swingCost * 2 && swing)
        {
            if ((swordData.selected[0] && !wMode) || (swordData.selected[1]&&wMode))
            {
                GetComponent<PlayerController>().stamina -= swingCost;
                GetComponent<PlayerController>().mana -= swingCost;
                c_Animator.SetTrigger("Heavy");
                StartCoroutine(PlayAnimation());
            }
        }
        if (Input.GetKeyDown("r"))
        {
            wMode = !wMode;
            
            StartCoroutine(Switch());
        }

        currentMoves = 0;
        for (int i = 0; i < _Tog.Length; i++)
        {
            if(_Tog[i].isOn)
            {
                currentMoves += ints[i];
            }
        }
        for (int i = 0; i < _Tog.Length; i++)
        {
            
            if (currentMoves + ints[i] > totalMoves && !_Tog[i].isOn)
            {
                
                _Tog[i].enabled = false;
            }
            else 
            {
                _Tog[i].enabled = true;
            }
            c_Animator.SetBool(_Attacks[i], _Tog[i].isOn);
        }
        
    }

    void UIManger()
    {
        
    }

    void ResetAnim()
    {
        c_Animator.ResetTrigger("Swing");
        c_Animator.ResetTrigger("Heavy");
    }

    IEnumerator PlayAnimation()
    {
        swing = false;
        AnimatorStateInfo stateInfo = c_Animator.GetCurrentAnimatorStateInfo(0);
        int cur = stateInfo.shortNameHash;
        
        yield return new WaitUntil(() => c_Animator.GetCurrentAnimatorStateInfo(0).shortNameHash != cur);
        stateInfo = c_Animator.GetCurrentAnimatorStateInfo(0);
        c_Animator.SetFloat("Sides", 0);
        c_Animator.SetFloat("Forward", 0);
        GetComponent<PlayerController>().speed = 0;
        GetComponent<PlayerController>().canMove = false;
        sw1.enabled = true;
        sw2.enabled = true;
        sw3.enabled = true;
        yield return new WaitForSeconds(stateInfo.length);
        GetComponent<PlayerController>().speed = 6;
        GetComponent<PlayerController>().canMove = true;
        sw1.enabled = false;
        sw2.enabled = false;
        sw3.enabled = false;
        swing = true;
        yield return null;
    }

    IEnumerator Switch()
    {
        ResetAnim();
        swing = false;
        GetComponent<PlayerController>().canSprint = false;
        GetComponent<PlayerController>().speed = 4;
        yield return new WaitForSeconds(1.15f);
        swing = true;
        GetComponent<PlayerController>().speed = 6;
        GetComponent<PlayerController>().canSprint = true;
    }
}
