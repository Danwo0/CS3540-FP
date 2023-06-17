using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public int baseDamage = 10;
    public float baseSpeed = 25f;
    public float baseGrowRate = 1f;
    public float baseDistance = 125f;
    
    public Vector3 origin;

    private float damageScale = 1.0f;
    private float speedScale = 1.0f;
    private float sizeScale = 1.0f;
    private float growScale = 1.0f;
    private float distanceScale = 1.0f;

    private int finalDamage;
    private float finalSpeed;
    private float finalGrow;
    private float finalDistance;
    private float duration;

    private bool isReady = false;
    private bool isFired = false;
    void Start()
    {
        origin = transform.position;
    }

    private void Update()
    {
        if (isFired)
        {
            transform.localScale *= finalGrow;
        }
        else if (isReady)
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            duration  = finalDistance / finalSpeed;

            rb.AddForce(transform.forward * finalSpeed, ForceMode.VelocityChange);

            isFired = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Astronaut") || collision.gameObject.CompareTag("Robot"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(baseDamage);
        }
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    public void SetProperties
    (float newDamageScale, float newSpeedScale, float newSizeScale, float newGrowScale,float newDistance)
    {
        damageScale = newDamageScale;
        speedScale = newSpeedScale;
        sizeScale = newSizeScale;
        growScale = newGrowScale;
        distanceScale = newDistance;

        UpdateProjectile();
    }

    private void UpdateProjectile()
    {
        finalDamage = Mathf.RoundToInt(baseDamage * damageScale);
        finalSpeed = baseSpeed * speedScale;
        finalGrow = baseGrowRate * growScale;
        finalDistance = baseDistance * distanceScale;
        
        // damage is damage, no action needed
        // speed is initial velocity, no action needed
        // size modifies scale
        transform.localScale *= sizeScale;
        // growth is updated per frame
        // distance determines duration of projectile in air
        duration = finalDistance / finalSpeed;

    }

    public void SetReady(bool status)
    {
        isReady = status;
    }
}
