using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    public float armor;
    public float reducePerc;
    public float damageReducePerc;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        damageReducePerc = 0;
        reducePerc = 1;
        for (int i = 0; i < armor; i++)
        {
            damageReducePerc += reducePerc;
            reducePerc = reducePerc * 0.99f;
        }
    }
}
