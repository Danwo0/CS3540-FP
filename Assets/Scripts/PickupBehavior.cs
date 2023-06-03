using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    public float duration = 60f;
    public string pickupType = "";
    public AudioClip pickupSFX;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(pickupSFX, transform.position);

            other.GetComponent<PlayerController>().setCurrentItem(pickupType);

            Destroy(gameObject);
        }
    }   
}
