using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Projectile"))
        {
            Debug.Log("Hit!");
            Destroy(transform.parent.gameObject);
        }
    }
}
