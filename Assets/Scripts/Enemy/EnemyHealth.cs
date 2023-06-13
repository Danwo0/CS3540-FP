using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public Slider healthBar;

    public int currentHealth;

    void Awake()
    {
        healthBar = GetComponentInChildren<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
            healthBar.value = currentHealth;
        }

        Debug.Log("Current Health:" + currentHealth);
    }
}