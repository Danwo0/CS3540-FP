using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("AlertDanger").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (AlertCounter.alertedCounter > 0 || ProxAlertCounter.alertedCounter > 0)
        {
            transform.Find("AlertDanger").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("AlertDanger").gameObject.SetActive(false);
        }
    }
}
