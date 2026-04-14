using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{

    void Update()
    {
        transform.position += transform.InverseTransformDirection(Vector3.back) * Time.deltaTime;
    }
}
