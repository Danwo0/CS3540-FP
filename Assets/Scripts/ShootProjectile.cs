using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShootProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject meleePrefab;

    public float projectileSpeed = 100f;
    public float meleeSpeed = 0f;

    public AudioClip projectileSFX;
    public AudioClip meleeSFX;

    public Image selectionImage;
    public Sprite gunSprite;
    public Sprite meleeSprite;

    int type;

    // Start is called before the first frame update
    void Start()
    {
        type = 0;
        selectionImage.sprite = gunSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            type = (type + 1) % 2;
            if (type == 0)
            {
                selectionImage.sprite = gunSprite;
            } else if (type == 1)
            {
                selectionImage.sprite = meleeSprite;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (type == 0)
            {
                fireProjectile();
            } else if (type == 1)
            {
                fireMelee();
            }
        }
    }

    void fireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation) as GameObject;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);

        AudioSource.PlayClipAtPoint(projectileSFX, transform.position);
    }
    void fireMelee()
    {
        GameObject projectile = Instantiate(meleePrefab, transform.position + transform.forward, transform.rotation) as GameObject;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);

        AudioSource.PlayClipAtPoint(meleeSFX, transform.position);
    }
}