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
    
    private int layerMask;
    private float rayLength;

    private Animator anim;

    private void Start()
    {
        timeBtwAttacks = ame.timeBtwAtk;
        currentAttacksToStun = ame.attacksToStun;
        anim = GetComponent<Animator>();
        firePos = gameObject.transform.GetChild(0);
        ameHealth = ame.health;
        currentAmeHealth = ameHealth;
        
        layerMask = LayerMask.GetMask("Default");
        rayLength = 50f;
    }

    private void Update()
    {
        Vector2 rayOrigin = transform.position;

        Vector2 rayDirection = Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, layerMask);
        if (hit.collider != null && hit.collider.CompareTag("Enemy") && timeBtwAttacks <= 0)
        {
            anim.Play("Attack");
            Invoke("BulletInstantiate", 0.3f);
            timeBtwAttacks = ame.timeBtwAtk;
        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }

        if (currentAmeHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void BulletInstantiate()
    {
        Instantiate(ame.bulletPref, firePos.transform.position, Quaternion.identity);
    }

    public void GetDamage(float amount)
    {
        currentAmeHealth -= amount;
    }
}
