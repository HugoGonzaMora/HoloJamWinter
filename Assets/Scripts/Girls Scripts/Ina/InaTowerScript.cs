using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InaTowerScript : MonoBehaviour
{
    public TowerType ina;
    private float timeBtwAttacks;

    private Animator anim;

    private Vector3 enemyPos;
    private float currentInaHealth;
    private float inaHealth;
    private int layerMask;
    private float rayLength;
    private int reflectDamage;

    private GameObject enemy;

    private void Start()
    {
        timeBtwAttacks = ina.timeBtwAtk;
        anim = GetComponent<Animator>();
        inaHealth = ina.health;
        currentInaHealth = inaHealth;
        reflectDamage = ina.reflectedDamage;
        
        layerMask = LayerMask.GetMask("Default");
        rayLength = 50f;
    }

    private void Update()
    {
        Vector2 rayOrigin = transform.position;

        Vector2 rayDirectionRight = Vector2.right;
        Vector2 rayDirectionLeft = Vector2.left;

        RaycastHit2D hitRight = Physics2D.Raycast(rayOrigin, rayDirectionRight, rayLength, layerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(rayOrigin, rayDirectionLeft, rayLength, layerMask);
        
        if (hitRight.collider != null && hitRight.collider.CompareTag("Enemy"))
        {
                enemyPos = hitRight.transform.position;
        } 
        if (hitLeft.collider != null && hitLeft.collider.CompareTag("Enemy"))
        {
                enemyPos = hitLeft.transform.position;
        }

        ////////////////////////////////////////////////////////////
        
        if (timeBtwAttacks <= 0 && (hitRight.collider != null || hitLeft.collider != null))
        {
            Invoke("BulletInstantiate", 0.3f);
            
            timeBtwAttacks = ina.timeBtwAtk;
        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }

        if (currentInaHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemy = collision.gameObject;
        }
    }

    private void BulletInstantiate()
    {
        Instantiate(ina.tentaclePref, enemyPos, Quaternion.identity);
    }

    public void GetDamage(float amount)
    {
        currentInaHealth -= amount;
        enemy.gameObject.GetComponent<EnemyController>()?.GetReflectDamage(reflectDamage);
    }
}
