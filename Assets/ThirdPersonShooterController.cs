using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ThirdPersonShooterController : MonoBehaviour
{
    public CinemachineVirtualCamera aimVirtualCamera;
    public LayerMask aimColliderMask = new LayerMask();

    public Transform projectileOrigin;
    public GameObject projectilePrefab;
    public GameObject meleePrefab;
    
    public AudioClip projectileSFX;
    public AudioClip meleeSFX;

    public Image selectionImage;
    public Sprite gunSprite;
    public Sprite meleeSprite;
    
    public Transform debug;

    public float normalSensitivity = 1f;
    public float aimSensitivity = 0.6f;
    public float projectileSpeed = 100f;

    private Animator anim;
    
    private MouseLook mouseLook;
    private ThirdPersonController controller;
    private Vector3 mouseWorldPosition;
    
    private bool isAiming;
    private int weaponType;

    void Start()
    {
        mouseLook = GetComponent<MouseLook>();
        controller = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
        
        isAiming = false;
        weaponType = 0;
        selectionImage.sprite = gunSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.isGameOver) return;
        
        anim.SetBool("attackRanged", false);
        anim.SetBool("attackMelee", false);
        mouseWorldPosition = Vector3.zero;

        UpdatePointer();
        Aiming();
        SwitchWeapon();
        Attack();
    }

    void UpdatePointer()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            debug.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }
    }

    void Aiming()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            aimVirtualCamera.gameObject.SetActive(true);
            mouseLook.SetSensitivity(aimSensitivity);
            controller.SetRotateOnMove(false);
            isAiming = true;

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 10f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            mouseLook.SetSensitivity(normalSensitivity);
            controller.SetRotateOnMove(true);
            isAiming = false;
        }
    }

    void SwitchWeapon()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            weaponType = (weaponType + 1) % 2;
            if (weaponType == 0)
            {
                selectionImage.sprite = gunSprite;
            }
            else if (weaponType == 1)
            {
                selectionImage.sprite = meleeSprite;
            }
        }
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (weaponType == 0)
            {
                anim.SetBool("attackRanged", true);
                Invoke("FireProjectile", 0.2f);
            }
            else if (weaponType == 1)
            {
                anim.SetBool("attackMelee", true);
                Invoke("FireMelee", 0.2f);
            }
        }
    }
    
    void FireProjectile()
    {
        Quaternion bulletDirection = Quaternion.identity;
        if (isAiming)
        {
            Vector3 aimDirection = (mouseWorldPosition - projectileOrigin.position).normalized;
            bulletDirection = Quaternion.LookRotation(aimDirection, Vector3.up);
        }
        else
        {
            bulletDirection = transform.rotation;
        }
        
        GameObject projectile = Instantiate
            (projectilePrefab, projectileOrigin.position, bulletDirection) as GameObject;
            
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(projectile.transform.forward * projectileSpeed, ForceMode.VelocityChange);

        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);

        AudioSource.PlayClipAtPoint(projectileSFX, transform.position);
    }

    void FireMelee()
    {
        GameObject projectile = Instantiate
            (meleePrefab, projectileOrigin.position + transform.forward * 2, transform.rotation) as GameObject;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward, ForceMode.VelocityChange);

        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);

        AudioSource.PlayClipAtPoint(meleeSFX, transform.position);
    }
}
