using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public struct SceneStatus
{
    public bool[] doorsOpen;
}
public class SceneController : MonoBehaviour
{
    public SceneStatus sceneData;
    public bool inRange;
    public GameObject _text;
    public GameObject pause;
    public GameObject fullPause;
    public GameObject mInput;

    public GameObject[] enemies;
    public GameObject[] enemiesReset;

    public GameObject[] doors;
    GameObject arrows;
    public GameObject player;

    public bool reset;

    string filePath;
    const string FILE_NAME = "SceneStatus.json";
    public static SceneController sCInstance { get; private set; }

    void Awake()
    {
        if (sCInstance != null && sCInstance != this)
        {
            Destroy(this);
        }
        else
        {
            sCInstance = this;
        }
    }

    public void ResetData()
    {
        sceneData.doorsOpen = new bool[3];
        for (int i = 0; i < sceneData.doorsOpen.Length; i++)
        {
            sceneData.doorsOpen[i] = false;
        }
        SaveData();
    }

    public void SaveData()
    {
        //gameStatus.startPos = transform.position;
        //StatisticalData.statInstance.SetStatus(playerData.currentLevel);
        string gameStatusJson = JsonUtility.ToJson(sceneData);
        File.WriteAllText(filePath + "/" + FILE_NAME, gameStatusJson);
    }

    public void LoadData()
    {
        if (File.Exists(filePath + "/" + FILE_NAME))
        {
            string loadedJson = File.ReadAllText(filePath + "/" + FILE_NAME);
            sceneData = JsonUtility.FromJson<SceneStatus>(loadedJson);
        }
        else
        {
            return;
            //ResetStatus();
        }
        for (int i = 0; i < doors.Length; i++)
        {
            if(doors[i].GetComponent<Door>() != null)
            {
                doors[i].GetComponent<Door>().isOpen = sceneData.doorsOpen[i];
            }
        }
        // Debug.Log("a " + swordData.unlocked[0]);
    }

    public void SetData(int _door)
    {
        sceneData.doorsOpen[_door] = true;
        //  Debug.Log("e " + swordData.unlocked[0]);
        SaveData();
    }


    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath;
        sceneData = new SceneStatus();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        doors = GameObject.FindGameObjectsWithTag("Door");
        NewEnemies();
        player = GameObject.FindWithTag("Player");
        /*if(reset)
        {
            reset = false;
            player.GetComponent<PlayerController>().ResetData();
            player.GetComponent<Sword>().ResetData();
        }*/
        inRange = false;
        //ResetData();
        LoadData();
    }



    void NewEnemies()
    {
        enemiesReset = new GameObject[enemies.Length];
        for(int i = 0; i < enemies.Length; i++)
        {
            enemiesReset[i] = Instantiate(enemies[i], enemies[i].transform.position, enemies[i].transform.rotation, enemies[i].transform.parent);
            enemiesReset[i].SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            inRange = true;
            _text.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
            _text.SetActive(false);
        }
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pause.SetActive(true);
        if (inRange)
        {
            fullPause.SetActive(true);
            Time.timeScale = 0;
            //enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemiesReset.Length; i++)
            {
                if (enemies[i] != null)
                {
                    Destroy(enemies[i]);
                }
                enemies[i] = enemiesReset[i];
                enemies[i].SetActive(true);
            }
            NewEnemies();
            player.GetComponent<PlayerController>().SetPosition();
            player.GetComponent<Sword>().swing = false;
            player.GetComponent<PlayerController>().Reset();
        }
        // mInput.SetActive(false);
        
       // player.GetComponent<Bow>().arrows = player.GetComponent<Bow>().maxArrows;
    }



    public void UnPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<Sword>().swing = true;
        Time.timeScale = 1;
        pause.SetActive(false);
        fullPause.SetActive(false);
       // mInput.SetActive(true);
    }

    void Reset()
    {
        if(player == null)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    public void LoadA(string scenename)
    {
        UnPause();
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(scenename);
    }

    public void Exit()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    void Update()
    {
        Reset();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
}
