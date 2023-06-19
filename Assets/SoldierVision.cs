using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierVision : MonoBehaviour
{
    private SoldierAI aiScript;
    private VisionAdjuster indicatorScript;
    
    void Start()
    {
        aiScript = transform.parent.gameObject.GetComponent<SoldierAI>();
        indicatorScript = transform.GetChild(0).gameObject.GetComponent<VisionAdjuster>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Entered");
            aiScript.PlayerSeen(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Exited");
            aiScript.PlayerSeen(false);
        }
    }

    public void ToggleIndicator(bool state)
    {
        indicatorScript.ToggleIndicator(state);
    }
}
