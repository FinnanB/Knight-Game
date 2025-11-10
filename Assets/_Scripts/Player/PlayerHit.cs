using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public float damage;

    public GameObject _Player;


    void Start()
    {
        _Player = GameObject.FindWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Enemy")
        {
            Debug.Log(other.gameObject);
            other.GetComponent<EnemyController>().Hit(damage);
            _Player.GetComponent<PlayerController>().mana++;
        }
        if (other.tag == "Boss")
        {
            other.GetComponent<SnakeController>().Hit(damage);
            _Player.GetComponent<PlayerController>().mana++;
        }
    }
}
