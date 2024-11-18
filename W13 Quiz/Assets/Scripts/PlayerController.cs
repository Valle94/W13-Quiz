using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Shooting")]
    [SerializeField] GameObject playerProjectile;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float projectileLifetime = 5f;


    Vector2 minBounds;
    Vector2 maxBounds;

    void Start()
    {
        InitBounds();
    }

    void Update()
    {
        // Calling movement method, and if space is pressed firing method
        // One press = one laser
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    // Trigger for when the player is hit by an enemy laser
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyLaser")
        {
            Debug.Log("Hit");
            Destroy(other.gameObject); //Destroy Enemy laser
            Destroy(gameObject); //Destroy Self
        }
    }
    
    //Movement method
    void Move()
    {
        //Getting user input as variable
        float x_pos = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //Clamping x position based on camera bounds
        float clampedX = Mathf.Clamp(transform.position.x + x_pos, minBounds.x, maxBounds.x);
        //Moving positions based on clamped variable
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);    
    }

    //Initialize the boundary of our screen space based on the camera view.
    void InitBounds()
    {
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    //Firing Method
    void Fire()
    {
        //When called, instantiate a player laser at player position
        GameObject instance = Instantiate(playerProjectile, 
                                            transform.position, 
                                            Quaternion.identity);

        //Use GetComponent to determine if the created instance has a rigidbody
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        //If it does, start moving it upwards
        if (rb != null)
        {
            rb.velocity = transform.up * projectileSpeed;
        }

        //Destroy the instance after a certain amount of time
        Destroy(instance, projectileLifetime);
    }
}
