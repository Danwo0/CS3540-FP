using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchBehavior : MonoBehaviour
{
    public Light lightSource;

    private bool switchOn;
    private bool playerDetected;
    void Start()
    {
        switchOn = true;
        playerDetected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerDetected) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            switchOn = !switchOn;
            lightSource.intensity = switchOn ? 1.0f : 0.2f;
        }
    }

    public void SetPlayerDetected(bool newPlayerDetected)
    {
        playerDetected = newPlayerDetected;
    }
}
