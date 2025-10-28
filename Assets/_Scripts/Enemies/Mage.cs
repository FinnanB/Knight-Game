using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mage : MonoBehaviour
{
    public Transform target;
    public GameObject magPrefab;
    public bool knocked;

    //public GameObject[] magic;

    public List<GameObject> magic;

    public Transform spawn;

    public bool launch;
    // Start is called before the first frame update
    void Start()
    {
        launch = true;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (knocked)
        {
            for (int i = magic.Count - 1; i >= 0; i--)
            {
                GameObject _magic = magic[i];
                magic.Remove(_magic);
                Destroy(_magic);
            }
            StopAllCoroutines();
            launch = true;
        }
        Order();
        bool seen = GetComponent<EnemyController>().isVisible;
        bool inRange = GetComponent<EnemyController>().isClose;
        if (magic.Count < 2 && launch)
        {
            //Debug.Log(launch);
            StartCoroutine(Spawn());
        }
        else if(launch && seen && inRange)
        {
            for (int i = magic.Count - 1; i >= 0; i--)
            {
                StartCoroutine(Launch(magic[i]));
            }
        }
        
    }

    IEnumerator Launch(GameObject mag)
    {
        launch = false;
        yield return new WaitForSeconds(1f);
        mag.GetComponent<Magic>().launch = true;
        magic.Remove(mag);
        yield return new WaitForSeconds(1f);
        launch = true;
    }

    IEnumerator Spawn()
    {
        
        launch = false;
        GetComponent<NavMeshAgent>().angularSpeed = 40;
        GetComponent<NavMeshAgent>().speed = 0.5f;
        yield return new WaitForSeconds(1f);
        GameObject arrow = Instantiate(magPrefab, spawn.position, spawn.rotation, transform);
        yield return new WaitForSeconds(0.01f);
        arrow.transform.SetParent(spawn.transform);
        magic.Add(arrow);
        GetComponent<NavMeshAgent>().angularSpeed = 180;
        GetComponent<NavMeshAgent>().speed = 3.5f;
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

    void Look(GameObject mag)
    {
        if (GetComponent<EnemyController>().seen)
        {
            mag.transform.LookAt(target);
        }
        else
        {
            mag.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
        }

    }
}
