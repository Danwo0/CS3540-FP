using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeAlerter : MonoBehaviour
{
    public GameObject UIElement;

    private void Start()
    {
        UIElement = GameObject.FindGameObjectWithTag("AlertImage");
        UIElement.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIElement.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIElement.SetActive(false);
        }
    }
}
