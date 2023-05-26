using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float despawnTimer = 3.0f;
    
    void Start()
    {
        Destroy(gameObject, despawnTimer);
    }
}
