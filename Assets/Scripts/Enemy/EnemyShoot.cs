using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 100f;
    public Transform playerTarget;

    private bool playerDetected;
    private float shootInterval = 0.5f;
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
            Debug.Log("Player entered");
            playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left");
            playerDetected = false;
        }
    }
}
