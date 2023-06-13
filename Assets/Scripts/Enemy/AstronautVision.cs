
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautVision : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Entered");
            transform.parent.gameObject.GetComponent<AstronautAI>().playerSeen();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Exited");
            transform.parent.gameObject.GetComponent<AstronautAI>().playerLost();
        }
    }
}
