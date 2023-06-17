using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeBehavior : MonoBehaviour
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerHealth = other.GetComponent<Collider>().GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }
}
