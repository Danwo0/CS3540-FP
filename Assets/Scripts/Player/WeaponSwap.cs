using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    public AudioClip swapSFX;

    public GameObject[] weaponDisplays;
    public GameObject[] projectilePrefabs;
    public int[] projectileSpeeds;
    public AudioClip[] projectileSFXs;
    public int[] projectileDamages;

    public static GameObject projectilePrefab;
    public static int projectileSpeed;
    public static AudioClip projectileSFX;
    public static int projectileDamage;

    int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentIndex < projectilePrefabs.Length - 1)
            {
                currentIndex++;
                AudioSource.PlayClipAtPoint(swapSFX, transform.position);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                AudioSource.PlayClipAtPoint(swapSFX, transform.position);
            }
        }

        for (int i = 0; i < weaponDisplays.Length; i++)
        {
            if (i != currentIndex)
            {
                weaponDisplays[i].gameObject.SetActive(false);
            }
        }

        weaponDisplays[currentIndex].SetActive(true);

        projectilePrefab = projectilePrefabs[currentIndex];
        projectileSpeed = projectileSpeeds[currentIndex];
        projectileSFX = projectileSFXs[currentIndex];
        projectileDamage = projectileDamages[currentIndex];
    }
}