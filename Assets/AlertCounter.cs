using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertCounter : MonoBehaviour
{
    public static int alertedCounter;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            alertedCounter++;
            Debug.Log("current count: " + alertedCounter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            alertedCounter--;
            Debug.Log("current count: " + alertedCounter);
        }
    }

    private void OnDestroy()
    {
        alertedCounter--;
        Debug.Log("current count: " + alertedCounter);
    }
}
