using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class HordeSceneController : MonoBehaviour
{
    public bool inRange;
    public GameObject _text;
    public GameObject pause;
    public GameObject fullPause;
    public GameObject mInput;

    public GameObject[] enemies;
    public GameObject player;

    public bool reset;

    public Transform[] spawnPos;

    public float spawnTime;
    public float spawnRate;


    // Start is called before the first frame update
    void Start()
    {
        NewEnemies();
        player = GameObject.FindWithTag("Player");
        /*if(reset)
        {
            reset = false;
            player.GetComponent<PlayerController>().ResetData();
            player.GetComponent<Sword>().ResetData();
        }*/
        inRange = false;
    }



    void NewEnemies()
    {
        GameObject[] curEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(curEnemies.Length <= 60)
        {
            int spawn = Random.Range(0, enemies.Length);
            int spawnP = Random.Range(0, spawnPos.Length);
            Instantiate(enemies[spawn], spawnPos[spawnP].position, spawnPos[spawnP].rotation, spawnPos[spawnP]);
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
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
            player.GetComponent<PlayerController>().SetPosition();
            player.GetComponent<Sword>().swing = false;
            player.GetComponent<PlayerController>().Reset();
        }
        // mInput.SetActive(false);

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
        if (player == null)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    public void LoadA(string scenename)
    {
        UnPause();
        if (scenename == "Menu")
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        SceneManager.LoadScene(scenename, LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    void Update()
    {
        if(Time.time > spawnTime)
        {
            NewEnemies();
            spawnTime = Time.time + spawnRate;
        }
        Reset();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
}

