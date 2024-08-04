using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour, ITower
{
    public TowerType towerType;
    protected float currentTowerHealth;
    protected float towerTimeBtwAttacs;
    protected int towerCost;
    protected int towerDamage;
    
    void Start()
    {
        Initialize();
    }

    void Update()
    {
        CheckTowerHP();
    }

    public virtual void Initialize()
    {
        currentTowerHealth = towerType.health;
        towerTimeBtwAttacs = towerType.timeBtwAtk;
        towerCost = towerType.cost;
        towerDamage = towerType.damage;
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void GetDamage(float amount)
    {
        currentTowerHealth -= amount;
    }

    public void CheckTowerHP()
    {
        if (currentTowerHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SellTower()
    {
        GameManager.Instance.holoPointsCnt += Convert.ToInt32(towerType.cost / 2);
        GameManager.Instance.UpdateHoloPoints();
        Destroy(gameObject);
    }

    protected void OnMouseDown()
    {
        SellTower();
    }
}
