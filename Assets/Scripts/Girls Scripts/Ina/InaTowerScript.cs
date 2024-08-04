using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InaTowerScript : BaseTower
{
    private Vector3 enemyPos;
    private int layerMask;
    private float rayLength;
    private int reflectDamage;

    private GameObject enemy;

    private void Start()
    {
        Initialize();
        
        reflectDamage = towerType.reflectedDamage;
        
        layerMask = LayerMask.GetMask("EnemyLayer");
        rayLength = 50f;
    }

    private void Update()
    {
        CheckTowerHP();
        
        Attack();
    }

    private void Attack()
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
        
        if (towerTimeBtwAttacs <= 0 && (hitRight.collider != null || hitLeft.collider != null))
        {
            Invoke("BulletInstantiate", 0.3f);
            
            towerTimeBtwAttacs = towerType.timeBtwAtk;
        }
        else
        {
            towerTimeBtwAttacs -= Time.deltaTime;
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
        Instantiate(towerType.tentaclePref, enemyPos, Quaternion.identity);
    }

    private void OnMouseDown()
    {
        SellTower();
    }
}
