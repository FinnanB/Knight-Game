using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public float damage;
    public float _D;

    public GameObject enemy;

    void Start()
    {
        enemy = transform.root.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().Hit(damage);
            GetComponent<Collider>().enabled = false;
            if(other.GetComponent<PlayerController>()._block == 0)
            {
                enemy.GetComponent<EnemyController>().sturdy = 0;
            }
        }
    }
}
