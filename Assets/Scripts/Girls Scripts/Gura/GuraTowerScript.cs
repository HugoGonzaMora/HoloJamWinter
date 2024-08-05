using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuraTowerScript : BaseTower
{
    private Transform firePos;
    
    private float _bleedDamage;
    private float _meeleDamage;
    
    private int layerMask;
    private float rayLength;
    
    private bool isInMeele = false;

    private GameObject target;

    private IEnumerator BleedCoroutine;

    private void Start()
    {
        Initialize();
        
        firePos = gameObject.transform.GetChild(0);
        
        _meeleDamage = towerType.meleeDamage;
        _bleedDamage = towerType.bleedingDamage;
        
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
        if (hit.collider != null && hit.collider.CompareTag("Enemy") && towerTimeBtwAttacs <= 0 && isInMeele == false)
        {
            anim.Play("RangedAttack");
            Invoke("BulletInstantiate", 0.3f);
            towerTimeBtwAttacs = towerType.timeBtwAtk;
        }
        else if (towerTimeBtwAttacs <= 0 && isInMeele == true)
        {
            anim.Play("MeleeAttack");
            target.GetComponent<EnemyController>()?.GetDamage(_meeleDamage);
            if (target.activeInHierarchy)
            {
                BleedCoroutine = target.GetComponent<EnemyController>()?.GetPassiveDamage(_bleedDamage);
                target.GetComponent<EnemyController>()?.StopCoroutine(BleedCoroutine);
    
                target.GetComponent<EnemyController>()?.StartCoroutine(BleedCoroutine);
            }
            towerTimeBtwAttacs = towerType.timeBtwAtk;
        }
        else
        {
            towerTimeBtwAttacs -= Time.deltaTime;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            isInMeele = true;
            target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
             isInMeele = false;
        }
    }

    private void BulletInstantiate()
    {
        Instantiate(towerType.waterballPref, firePos.transform.position, Quaternion.identity);
    }

    private void OnMouseDown()
    {
        SellTower();
    }
}
