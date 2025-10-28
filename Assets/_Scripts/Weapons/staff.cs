using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staff : MonoBehaviour
{
    public Transform target;
    public GameObject magPrefab;

    //public GameObject[] magic;

    public List<GameObject> magic;

    public Transform spawn;

    bool launch;
    // Start is called before the first frame update
    void Start()
    {
        launch = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1) && launch)
        {
            for(int i = magic.Count - 1; i >= 0; i--)
            {
                StartCoroutine(Launch(magic[i]));
            }
        }
        else if(Input.GetMouseButtonDown(1) && launch && magic.Count < 6 && !Input.GetMouseButtonDown(0) && GetComponent<PlayerController>().mana >= 1)
        {
            StartCoroutine(Spawn());
        }
        else if(Input.GetMouseButtonDown(0) && magic.Count != 0 && !Input.GetMouseButtonDown(1) && launch)
        {
            StartCoroutine(Launch(magic[magic.Count - 1]));
        }
        Order();
    }

    IEnumerator Launch(GameObject mag)
    {
        launch = false;
        
        mag.GetComponent<Magic>().launch = true;
        magic.Remove(mag);
        yield return new WaitForSeconds(0.2f);
        launch = true;
    }

    void Order()
    {
        int s = Mathf.RoundToInt(Mathf.Sqrt(magic.Count));
        //int s = 3;
        for (int i = 0; i < magic.Count; i++)
        {
            float x = i / s;
            int y = i % s;
            Vector3 tar = new Vector3(x - (s / 2), y, 0);
            magic[i].transform.localPosition = tar;
            Look(magic[i]);
        }
    }

    IEnumerator Spawn()
    {
        launch = false;
        GetComponent<PlayerController>().mana--;
        yield return new WaitForSeconds(1f);
        GameObject arrow = Instantiate(magPrefab, spawn.position, spawn.rotation, spawn);
        magic.Add(arrow);
        launch = true;
    }

    void Look(GameObject mag)
    {
        if(GetComponent<PlayerController>().lockOn)
        {
            target = GetComponent<PlayerController>().enemy;
            mag.transform.LookAt(target);
        }
        else
        {
            mag.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
        }
        
    }
}
