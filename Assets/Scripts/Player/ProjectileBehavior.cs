using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public int damage = 10;
    public float duration = 5f;
    public Vector3 origin;
    
    void Start()
    {
        Destroy(gameObject, duration);
        origin = transform.position;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Astronaut") || collision.gameObject.CompareTag("Robot"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
