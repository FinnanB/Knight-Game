using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public float damage;
    float trueDamage;

    public GameObject _Player;


    void Start()
    {
        _Player = GameObject.FindWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        trueDamage = damage * _Player.GetComponent<PlayerController>().playerData.damage;
        Debug.Log(trueDamage);
        if (other.tag == "Enemy")
        {
           // Debug.Log(other.gameObject);
            other.GetComponent<EnemyController>().Hit(trueDamage, _Player);
            _Player.GetComponent<PlayerController>().mana +=10;
        }
        if (other.tag == "Boss")
        {
            other.GetComponent<SnakeController>().Hit(trueDamage);
            _Player.GetComponent<PlayerController>().mana++;
        }
    }
}
