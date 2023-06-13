using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShootProjectile : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject projectile = Instantiate(WeaponSwap.projectilePrefab, transform.position + transform.forward, transform.rotation) as GameObject;

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * WeaponSwap.projectileSpeed, ForceMode.VelocityChange);

            projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);

            AudioSource.PlayClipAtPoint(WeaponSwap.projectileSFX, transform.position);
        }
    }
}