using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuraBulletScript : MonoBehaviour
{
    public float speed = 20f;
    public TowerType guraTowerSO;

    private float _damage;

    private void Start()
    {
        _damage = guraTowerSO.damage;
        Invoke("Destroy",1f);
    }

    void Update()
    {
        transform.Translate(Vector2.right * (speed * Time.deltaTime));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>()?.GetDamage(_damage);
            Destroy(this.gameObject);
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
