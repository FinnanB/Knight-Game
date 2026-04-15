using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonHover : Selectable
{
    public GameObject image1;

    void Update()
    {
        if(image1 != null)
        {
          //  image1.SetActive(IsHighlighted());
        }
    }
}
