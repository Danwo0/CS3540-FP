using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
        public float moveSpeed = 5f;
        public float jumpHeight = 2f;
        public float gravity = 9.81f;
        public float airControl = 10f;
        
        private CharacterController controller;
        private Vector3 input, moveDirection;
        
        void Start()
        {
            controller = GetComponent<CharacterController>();
        }
        
        void Update()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
    
            input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
    
            input *= moveSpeed;
    
            if (controller.isGrounded)
            {
                moveDirection = input;
                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                }
                else
                {
                    moveDirection.y = 0.0f;
                }
            }
            else
            {
                input.y = moveDirection.y;
                moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
            }
    
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);

            // crouching
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.localScale = 
                    Vector3.Lerp(transform.localScale, new Vector3(1, 0.5f, 1), Time.deltaTime * 10);
            }
            else
            {
                transform.localScale = 
                    Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 10);
            }
            Debug.Log(transform.localScale.ToString());
        }
}
