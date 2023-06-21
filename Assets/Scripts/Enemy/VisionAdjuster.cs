using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionAdjuster : MonoBehaviour
{
    private float angle;
    private float distance;

    private ConeCollider coneCollider;

    void Start()
    {
        coneCollider = transform.parent.gameObject.GetComponent<ConeCollider>();
        
        angle = coneCollider.m_angle;
        distance = coneCollider.m_distance;

    }
    void Update()
    {
        UpdateIndicator();
    }

    void UpdateIndicator()
    {
        angle = coneCollider.m_angle;
        distance = coneCollider.m_distance;
        float scale = 0.0025f * angle * angle + 0.305f * angle + 0.1f;

        transform.localScale = new Vector3(scale, scale, distance);
    }
}
