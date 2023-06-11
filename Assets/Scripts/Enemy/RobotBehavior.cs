using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehavior : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 100f;
    public Transform player;

    private bool playerDetected;
    private float shootInterval = 0.5f;
    private float timer; 

    void Start()
    {
        playerDetected = false;
        timer = 0;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (playerDetected && timer <= 0)
        {   
            GameObject bullet =
                Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation) as GameObject;
            
            bullet.transform.LookAt(player);
            
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
