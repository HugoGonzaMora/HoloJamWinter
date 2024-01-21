using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public TowerType kiaraTowerSO;
    public float speed = 15f;
    private float _damage;
    private float _fireDamage;
    private IEnumerator BurnCoroutine;

    private void Start()
    {
        Invoke("Destroy", 3f);
        _damage = kiaraTowerSO.damage;
        _fireDamage = kiaraTowerSO.fireDamage;
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
            
            BurnCoroutine = collision.gameObject.GetComponent<EnemyController>()?.GetBurn(_fireDamage);
            collision.gameObject.GetComponent<EnemyController>()?.StopCoroutine(BurnCoroutine);
    
            collision.gameObject.GetComponent<EnemyController>()?.StartCoroutine(BurnCoroutine);
            Destroy(this.gameObject);
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
