using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmeTowerScript : MonoBehaviour
{
    public TowerType ame;
    private Transform firePos;

    private float currentAmeHealth;
    private float ameHealth;
    private float timeBtwAttacks;
    private float currentAttacksToStun;

    private Animator anim;

    private void Start()
    {
        timeBtwAttacks = ame.timeBtwAtk;
        currentAttacksToStun = ame.attacksToStun;
        anim = GetComponent<Animator>();
        firePos = gameObject.transform.GetChild(0);
        ameHealth = ame.health;
        currentAmeHealth = ameHealth;
    }

    private void Update()
    {
        if (timeBtwAttacks <= 0)
        {
            anim.Play("Attack");
            Invoke("BulletInstantiate", 0.3f);
            timeBtwAttacks = ame.timeBtwAtk;
        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }
    }

    private void BulletInstantiate()
    {
        Instantiate(ame.bulletPref, firePos.transform.position, Quaternion.identity);
    }

    public void GetDamage(float amount)
    {
        ameHealth -= amount;
    }
}
