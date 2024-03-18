using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFarmController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 5f;
    private float vertical;
    private float horizontal;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal, vertical).normalized * speed;
    }
}
