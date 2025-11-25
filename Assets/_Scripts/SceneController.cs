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
    GameObject arrows;
    public GameObject player;

    public bool reset;
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.FindWithTag("Player");
        /*if(reset)
        {
            reset = false;
            player.GetComponent<PlayerController>().ResetData();
            player.GetComponent<Sword>().ResetData();
        }*/
        inRange = false;
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
        foreach(GameObject enemy in enemies)
        {
            enemy.SetActive(true);
            enemy.GetComponent<EnemyController>().Reset();
        }
        player.GetComponent<PlayerController>().SetPosition();
        player.GetComponent<PlayerController>().Reset();
       // player.GetComponent<Bow>().arrows = player.GetComponent<Bow>().maxArrows;
    }

    public void UnPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
