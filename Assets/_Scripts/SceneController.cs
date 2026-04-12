using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public bool inRange;
    public GameObject _text;
    public GameObject pause;
    public GameObject mInput;

    public GameObject[] enemies;
    public GameObject[] enemiesReset;
    GameObject arrows;
    public GameObject player;

    public bool reset;
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
        enemiesReset = new GameObject[enemies.Length];
        for(int i = 0; i < enemies.Length; i++)
        {
            enemiesReset[i] = Instantiate(enemies[i], enemies[i].transform.position, Quaternion.identity, enemies[i].transform.parent);
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
        // mInput.SetActive(false);
        pause.SetActive(true);
        Time.timeScale = 0;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
            enemies[i] = enemiesReset[i];
            enemies[i].SetActive(true);
        }
        NewEnemies();
        player.GetComponent<PlayerController>().SetPosition();
        player.GetComponent<Sword>().swing = false;
        player.GetComponent<PlayerController>().Reset();
       // player.GetComponent<Bow>().arrows = player.GetComponent<Bow>().maxArrows;
    }

    public void UnPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<Sword>().swing = true;
        Time.timeScale = 1;
        pause.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.Escape) && inRange)
        {
            Pause();
        }
    }
}
