using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CalliTowerScript : BaseTower
{
    private List<GameObject> enemiesInRange = new List<GameObject>();

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        CheckTowerHP();
        
        Attack();
    }

    protected override void Attack()
    {
        if (towerTimeBtwAttacs <= 0 && enemiesInRange.Any())
        {
            int randNum = Random.Range(1, 101);
            if (randNum == 3)
            {
                enemiesInRange[Random.Range(0, enemiesInRange.Count)].gameObject.GetComponent<EnemyController>()?.GetDamage(999);
            }
            anim.Play("Attack");
            foreach (GameObject enemy in enemiesInRange.ToList())
            {
                enemy.gameObject.GetComponent<EnemyController>()?.GetDamage(towerDamage);
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
            enemiesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.gameObject);
        }
    }
    
    private void OnMouseDown()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        BoxCollider2D mainCollider = GetComponent<BoxCollider2D>();

        if (mainCollider != null && !mainCollider.isTrigger && mainCollider.OverlapPoint(mousePosition))
        {
            SellTower();
        }
    }
}
