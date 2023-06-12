using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float duration = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!(other.collider.CompareTag("Player"))) Destroy(gameObject);
    }
}
