using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShootProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 300f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!LevelManager.isGameOver)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject projectile =
                    Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation) as
                        GameObject;

                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

                projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);

                // AudioSource.PlayClipAtPoint(spellSFX, transform.position);
            }
        }
    }
}