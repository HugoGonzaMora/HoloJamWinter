using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public float speed = 15f;
    void Update()
    {
        transform.Translate(Vector2.right * (speed * Time.deltaTime));
    }
}
