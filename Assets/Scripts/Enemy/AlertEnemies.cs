using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertEnemies : MonoBehaviour
{
    AstronautAI astronaut;
    RobotAI robot;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            astronaut = transform.parent.gameObject.GetComponent<AstronautAI>();
            robot = transform.parent.gameObject.GetComponent<RobotAI>();

            if (astronaut != null)
            {
                astronaut.alert();
            }

            if (robot != null)
            {
                robot.alert();
            }
        }
    }
}
