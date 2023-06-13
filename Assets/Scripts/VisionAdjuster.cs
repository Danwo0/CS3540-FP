using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionAdjuster : MonoBehaviour
{
    private float startingAngle;
    private float startingDistance;
    private float angle;
    private float distance;

    private bool prevLightOn;

    private ConeCollider coneCollider;

    void Start()
    {
        coneCollider = transform.parent.gameObject.GetComponent<ConeCollider>();
        
        startingAngle = coneCollider.m_angle;
        startingDistance = coneCollider.m_distance;
        angle = startingAngle;
        distance = startingDistance;

        prevLightOn = true;
    }
    void Update()
    {
        CheckLight();
        UpdateIndicator();
    }

    void CheckLight()
    {
        bool currentLightOn = LevelManager.isLightOn;

        if (currentLightOn == prevLightOn) return;
        
        if (currentLightOn)
        {
            coneCollider.m_angle = startingAngle;
            coneCollider.m_distance = startingDistance;

            angle = startingAngle;
            distance = startingDistance;
        }
        else
        {
            float newAngle = startingAngle * 1.5f;
            float newDistance = startingDistance * 0.5f;
            
            coneCollider.m_angle = newAngle;
            coneCollider.m_distance = newDistance;
            
            angle = newAngle;
            distance = newDistance;
        }

        prevLightOn = currentLightOn;
        UpdateConeCollider();
    }

    void UpdateConeCollider()
    {
        coneCollider.enabled = false;
        coneCollider.enabled = true;
    }
    
    void UpdateIndicator()
    {
        float scale = 0.0025f * angle * angle + 0.305f * angle + 0.1f;

        transform.localScale = new Vector3(scale, scale, distance);
    }
}
