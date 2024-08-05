using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmeTowerScript : BaseTower
{
    private Transform firePos;

    private float currentAttacksToStun;
    
    private int layerMask;
    private float rayLength;

    private void Start()
    {
        Initialize();
        
        currentAttacksToStun = towerType.attacksToStun;
        firePos = gameObject.transform.GetChild(0);
        
        layerMask = LayerMask.GetMask("EnemyLayer");
        rayLength = 50f;
    }

    private void Update()
    {
       CheckTowerHP();
       
       Attack();
    }

    protected override void Attack()
    {
        Vector2 rayOrigin = transform.position;

        Vector2 rayDirection = Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, layerMask);
        if (hit.collider != null && hit.collider.CompareTag("Enemy") && towerTimeBtwAttacs <= 0)
        {
            anim.Play("Attack");
            Invoke("BulletInstantiate", 0.3f);
            towerTimeBtwAttacs = towerType.timeBtwAtk;
        }
        else
        {
            towerTimeBtwAttacs -= Time.deltaTime;
        }
    }

    private void BulletInstantiate()
    {
        Instantiate(towerType.bulletPref, firePos.transform.position, Quaternion.identity);
    }

    private void OnMouseDown()
    {
        SellTower();
    }
}
