using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Firing")]
    [SerializeField] GameObject enemyProjectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime =5f;
    [SerializeField] float baseFiringRate = 0.2f;
    [SerializeField] float firingRateVariance = 0f;
    [SerializeField] float minimumFiringRate = 0.1f;

    void Start()
    {
        //Start firing coroutine
        StartCoroutine(FireRepeat());
    }

    void Update()
    {

    }

    //Trigger for when an enemy is hit by a player laser
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerLaser")
        {
            Debug.Log("Hit");
            Destroy(other.gameObject); //Destroy Player laser
            Destroy(gameObject); //Destroy Self
        }
    }

    // Firing coroutine
    IEnumerator FireRepeat()
    {
        while (true)
        {
            //Instantiate laser at enemy position
            GameObject instance = Instantiate(enemyProjectilePrefab, 
                                                transform.position, 
                                                Quaternion.identity);

            //Use GetComponent to determine if the created instance has a rigidbody
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            //If it does, start moving it downwards
            if (rb != null)
            {
                //Negative transform.up leads to downwards movement
                rb.velocity = -transform.up * projectileSpeed;
            }

            //Destroy the instance after a certain amount of time
            Destroy(instance, projectileLifetime);

            //Calculate a random firing rate to make enemies more dynamic
            float timeToNextProjectile = Random.Range(baseFiringRate - firingRateVariance,
                                                    baseFiringRate + firingRateVariance);
            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);

            //Wait for a number of seconds calculated above before repeating
            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
}
