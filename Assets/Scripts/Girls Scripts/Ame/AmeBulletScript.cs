using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmeBulletScript : MonoBehaviour
{
    public float speed = 20f;
    public TowerType AmeTowerSO;

    private float damage;

    private void Start()
    {
        damage = AmeTowerSO.damage;
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
            collision.gameObject.GetComponent<EnemyController>()?.GetDamage(damage);
            collision.gameObject.GetComponent<EnemyController>()?.AddStunBullet();
            
            Destroy(this.gameObject);
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
