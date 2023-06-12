using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehavior : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform playerTarget;

    public float bulletSpeed = 100f;
    public int damage = 30;
    public float shootInterval = 0.5f;
    
    private bool playerDetected;
    private float timer; 

    void Start()
    {
        playerDetected = false;
        timer = 0;
        if (playerTarget == null)
        {
            playerTarget = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (playerDetected && timer <= 0)
        {   
            GameObject bullet =
                Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation) as GameObject;
            
            bullet.GetComponent<EnemyBulletBehavior>().SetDamage(damage);
            bullet.transform.LookAt(playerTarget);
            
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(bullet.transform.forward * bulletSpeed, ForceMode.VelocityChange);
            
            bullet.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);
            timer = shootInterval;
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }
}
