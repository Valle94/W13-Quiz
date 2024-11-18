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
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }
    
    void Move()
    {
        float x_pos = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float clampedX = Mathf.Clamp(transform.position.x + x_pos, minBounds.x, maxBounds.x);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);    
    }

    //Initialize the boundary of our screen space based on the camera view.
    void InitBounds()
    {
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    void Fire()
    {
        GameObject instance = Instantiate(playerProjectile, 
                                            transform.position, 
                                            Quaternion.identity);

        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.up * projectileSpeed;
        }

        Destroy(instance, projectileLifetime);
    }
}
