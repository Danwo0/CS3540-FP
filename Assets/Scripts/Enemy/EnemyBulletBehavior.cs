using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehavior : MonoBehaviour
{
    public Transform player;
    public int damageAmount = 30;
    
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }    
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            var playerHealth = other.collider.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }

    public void SetDamage(int newDamage)
    {
        damageAmount = newDamage;
    }
}
