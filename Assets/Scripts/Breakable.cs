﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
   public GameObject cratePieces;
   public float explosionForce = 100f;
   public float explosionRadius = 10f;

   public GameObject[] loots;
   
   public void OnCollisionEnter(Collision collision)
   {
      if (collision.collider.CompareTag("Projectile"))
      {
         Transform currentCrate = gameObject.transform;

         GameObject pieces = Instantiate(cratePieces, currentCrate.position, currentCrate.rotation);
         pieces.transform.localScale = transform.localScale;

         Rigidbody[] rbPieces = pieces.GetComponentsInChildren<Rigidbody>();

         foreach (Rigidbody rb in rbPieces)
         {
            rb.AddExplosionForce(explosionForce, currentCrate.position, explosionRadius);
         }

         var value = Random.Range(0, 100);
         if (value < 25)
         {
            var length = loots.Length;

            Instantiate(loots[value % length], currentCrate.position, currentCrate.rotation);
         }

         Destroy(gameObject);
      }
   }
}
