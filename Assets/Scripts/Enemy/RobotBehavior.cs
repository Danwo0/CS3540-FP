using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehavior : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform playerTarget;

    public Transform barrel1;
    public Transform barrel2;

    public float bulletSpeed = 100f;
    public int damage = 30;
    public float shootInterval = 0.5f;
    
    private bool playerDetected;
    private float timer;
    private int barrel;

    void Start()
    {
        playerDetected = false;
        timer = 0;
        barrel = 0;
        if (playerTarget == null)
        {
            playerTarget = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
        }
    }

    void Update()
    {
        if (LevelManager.isGameOver) return; 
        
        timer -= Time.deltaTime;
        if (playerDetected && timer <= 0)
        {
            Transform bulletSource = barrel % 2 == 0 ? barrel1 : barrel2;
            barrel = (barrel + 1) % 2;
                
            GameObject bullet = Instantiate
                (bulletPrefab, bulletSource.position + bulletSource.forward, bulletSource.rotation) as GameObject;
            
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
