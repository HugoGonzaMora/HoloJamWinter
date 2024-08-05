using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiaraTowerScript : BaseTower
{
    private Transform firePos;
    
    private int layerMask;
    private float rayLength;

    private void Start()
    {
        Initialize();
        
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
            Invoke("FireballInstantiate", 0.4f);
            towerTimeBtwAttacs = towerType.timeBtwAtk;
        }
        else
        {
            towerTimeBtwAttacs -= Time.deltaTime;
        }
    }

    private void FireballInstantiate()
    {
        Instantiate(towerType.fireballPref, firePos.transform.position, Quaternion.identity);
    }
}
