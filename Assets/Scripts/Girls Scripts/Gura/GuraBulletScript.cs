using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuraBulletScript : MonoBehaviour
{
    public float speed = 20f;

    private void Start()
    {
        Invoke("Destroy",1f);
    }

    void Update()
    {
        transform.Translate(Vector2.right * (speed * Time.deltaTime));
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
