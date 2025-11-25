using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;

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

    public float swingCost;

    public bool changed;

    public List<string> _Attacks = new List<string>();
    public int[] ints = new int[3];
    public Toggle[] _Tog = new Toggle[3];
    public int currentMoves;
    public int totalMoves;

    public int lvl;

    public bool run;
    public TMP_Text moveCount;

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
    }

    public void ResetData()
    {
        swordData.level = 1;
        swordData.unlocked = new bool[3];
        swordData.selected = new bool[3];
        for (int i = 0; i < swordData.unlocked.Length; i++)
        {
            swordData.unlocked[i] = false;
            swordData.selected[i] = false;
        }
        SaveData();
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
        for (int i = 0; i < _Tog.Length; i++)
        {
            _Tog[i].isOn = swordData.selected[i];
            _Tog[i].gameObject.SetActive(swordData.unlocked[i]);
        }
    }

    public void SetData()
    {

        for (int i = 0; i < _Tog.Length; i++)
        {
            swordData.selected[i] = _Tog[i].isOn;
            swordData.unlocked[i] = _Tog[i].gameObject.activeInHierarchy;
        }
        SaveData();
    }

    public void SaveData()
    {
        //gameStatus.startPos = transform.position;
        //StatisticalData.statInstance.SetStatus(playerData.currentLevel);
        string gameStatusJson = JsonUtility.ToJson(swordData);
        File.WriteAllText(filePath + "/" + FILE_NAME, gameStatusJson);
    }



    void Update()
    {
        totalMoves = swordData.level;
        moveCount.text = "Attacks: " + currentMoves + "/" + totalMoves;
        if (run)
        {
            SetData();
            //Debug.Log("h");
            run = false;
        }
        if (Input.GetMouseButtonDown(0) && GetComponent<PlayerController>().stamina >= swingCost)
        {
            GetComponent<PlayerController>().stamina -= swingCost;
            c_Animator.SetTrigger("Swing");
        }
        if (Input.GetMouseButtonDown(1) && GetComponent<PlayerController>().stamina >= swingCost*2 && GetComponent<PlayerController>().mana >= swingCost * 2)
        {
            GetComponent<PlayerController>().stamina -= swingCost;
            GetComponent<PlayerController>().mana -= swingCost;
            c_Animator.SetTrigger("Heavy");
        }
        if (Input.GetKeyDown("r"))
        {
            c_Animator.SetTrigger("Switch");
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

    IEnumerator Switch()
    {
        while (changed)
        {
            yield return null;
        }
        ResetAnim();
        GetComponent<PlayerController>().speed = 4;
        while (!changed)
        {
            yield return null;
        }
        GetComponent<PlayerController>().speed = 6;
    }
}
