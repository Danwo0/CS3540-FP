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

    private MouseLook mouseLook;
    private ThirdPersonController controller;
    private Vector3 mouseWorldPosition;

    private int weaponType;

    void Start()
    {
        mouseLook = GetComponent<MouseLook>();
        controller = GetComponent<ThirdPersonController>();
        weaponType = 0;
        selectionImage.sprite = gunSprite;
    }

    // Update is called once per frame
    void Update()
    {
        mouseWorldPosition = Vector3.zero;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            debug.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }
        
        if (Input.GetKey(KeyCode.Mouse1))
        {
            aimVirtualCamera.gameObject.SetActive(true);
            mouseLook.SetSensitivity(aimSensitivity);
            controller.SetRotateOnMove(false);

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
        }

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

        if (Input.GetButtonDown("Fire1"))
        {
            if (weaponType == 0)
            {
                FireProjectile();
            }
            else if (weaponType == 1)
            {
                FireMelee();
            }
        }
    }

    void FireProjectile()
    {
        Vector3 aimDirection = (mouseWorldPosition - projectileOrigin.position).normalized;
        Quaternion bulletDirection = Quaternion.LookRotation(aimDirection, Vector3.up);
        Debug.DrawLine(projectileOrigin.position, aimDirection, Color.red);
        
        GameObject projectile = Instantiate(projectilePrefab, projectileOrigin.position, bulletDirection) as GameObject;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(projectile.transform.forward * projectileSpeed, ForceMode.VelocityChange);

        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);

        AudioSource.PlayClipAtPoint(projectileSFX, transform.position);
    }

    void FireMelee()
    {
        GameObject projectile = Instantiate(meleePrefab, projectileOrigin.position + transform.forward, transform.rotation) as GameObject;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        
        rb.AddForce(transform.forward, ForceMode.VelocityChange);

        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);

        AudioSource.PlayClipAtPoint(meleeSFX, transform.position);
    }
}
