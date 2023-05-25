using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 10;
    private Transform player;
    Vector3 offset;
    
    void Start()
    {
        player = transform.parent.transform;
        offset = player.transform.position - transform.position;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        player.Rotate(0, moveX, 0);

        float desiredAngle = player.eulerAngles.y;
        Quaternion q = Quaternion.Euler(0, desiredAngle, 0);
        transform.position = player.position - (q * offset);

        transform.LookAt(player);


    }
}
