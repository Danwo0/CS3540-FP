
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautVision : MonoBehaviour
{
    private AstronautAI aiScript;
    private VisionAdjuster indicatorScript;
    
    void Start()
    {
        aiScript = transform.parent.gameObject.GetComponent<AstronautAI>();
        indicatorScript = transform.GetChild(0).gameObject.GetComponent<VisionAdjuster>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Entered");
            aiScript.playerSeen();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Exited");
            aiScript.playerLost();
        }
    }
    
    public void ToggleIndicator(bool state)
    {
        indicatorScript.ToggleIndicator(state);
    }
}
