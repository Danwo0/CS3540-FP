using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeAlerter : MonoBehaviour
{
    public GameObject UIElement;

    int alertedCounter;

    private void Start()
    {
        UIElement = GameObject.FindGameObjectWithTag("AlertImage");
        UIElement.SetActive(false);
    }

    private void Update()
    {
        if (alertedCounter == 0)
        {
            UIElement.SetActive(false);
        } else if (alertedCounter > 0)
        {
            UIElement.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyVision"))
        {
            alertedCounter++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyVision"))
        {
            alertedCounter--;
        }
    }
}
