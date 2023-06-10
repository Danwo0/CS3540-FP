using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = 9.81f;
    public float airControl = 0.5f;
    public float acceleration = 5f;

    public GameObject cameraTarget;

    private CharacterController controller;
    private Animator anim;
    
    private float speedBoost = 1f;
    private float jumpBoost = 1f;
    
    private float angVelocity = 0f;
    private float speed = 0f;

    private bool isGrounded;
    private Vector3 moveDirection;
    private float rotateAngle;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //PowerUp();
        Setup();
        Jump();
        Move();
    }
    
    void Setup()
    {
        moveDirection = Vector3.zero;
        isGrounded = controller.isGrounded;
        anim.SetBool("isJumping", false);
    }
    
    void Jump()
    {
        // on the ground start jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            anim.SetBool("isJumping", true);
            //moveDirection.y = Mathf.Sqrt(2 * jumpBoost * gravity);
        } 
        // on the ground
        else if (isGrounded)
        {
            moveDirection.y = 0.0f;
        }
        // gravity as constant force
        moveDirection.y -= gravity;
    }
    
    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        float currentSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;
        Vector3 input = new Vector3(moveHorizontal, 0, moveVertical);
        
        bool isStopped = input.magnitude == 0;
        
        if (isStopped)
        {
            speed = 0.0f;
        }
        else
        {
            speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * 2 : moveSpeed;
        }

        anim.SetFloat("Speed", speed);
        input.Normalize();
        
        if (!isStopped)
        {
            rotateAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + 
                          Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle
                (transform.eulerAngles.y, rotateAngle, ref angVelocity, Time.deltaTime * 0.05f);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = (Quaternion.Euler(0.0f, rotateAngle, 0.0f) * Vector3.forward).normalized;
        
        if (!isGrounded)
        {
            targetDirection *= airControl;
        }
        
        moveDirection += targetDirection * speed * speedBoost;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
