using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    [SerializeField] protected TowerType towerType;
    
    protected float currentTowerHealth;
    protected float towerTimeBtwAttacs;
    protected int towerCost;
    protected int towerDamage;

    protected Animator anim;

    protected void Initialize()
    {
        currentTowerHealth = towerType.health;
        towerTimeBtwAttacs = towerType.timeBtwAtk;
        towerCost = towerType.cost;
        towerDamage = towerType.damage;
        anim = GetComponent<Animator>();
    }

    protected abstract void Attack();

    public void GetDamage(float amount)
    {
        currentTowerHealth -= amount;
    }

    protected void CheckTowerHP()
    {
        if (currentTowerHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected void SellTower()
    {
        GameManager.Instance.holoPointsCnt += Convert.ToInt32(towerType.cost / 2);
        GameManager.Instance.UpdateHoloPoints();
        Destroy(gameObject);
    }
}
