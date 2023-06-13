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
            astronaut = other.gameObject.GetComponent<AstronautAI>();
            robot = other.gameObject.GetComponent<RobotAI>();

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
