using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CasingSounds : MonoBehaviour
{
    void OnTriggerEnter()
    {
        this.GetComponent<AudioSource>().Play();
    }
}
