using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiaraTowerScript : MonoBehaviour
{
    public TowerType kiara;
    private Transform firePos;
    public float dynamicTimeBtwAttack;
    private float currentKiaraHealth;
    private float kiaraHealth;
    private float timeBtwAttck;
    
    private int layerMask;
    private float rayLength;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        timeBtwAttck = kiara.timeBtwAtk;
        dynamicTimeBtwAttack = kiara.timeBtwAtk;
        firePos = gameObject.transform.GetChild(0);
        kiaraHealth = kiara.health;
        currentKiaraHealth = kiaraHealth;
        
        layerMask = LayerMask.GetMask("Default");
        rayLength = 50f;
    }

    private void Update()
    {
        Vector2 rayOrigin = transform.position;

        Vector2 rayDirection = Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, layerMask);
        if (hit.collider != null && hit.collider.CompareTag("Enemy") && timeBtwAttck <= 0)
        {
            anim.Play("Attack");
            Invoke("FireballInstantiate", 0.4f);
            timeBtwAttck = dynamicTimeBtwAttack;
        }
        else
        {
            timeBtwAttck -= Time.deltaTime;
        }

        if (currentKiaraHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void FireballInstantiate()
    {
        Instantiate(kiara.fireballPref, firePos.transform.position, Quaternion.identity);
    }

    private void OnMouseDown()
    {
        Destroy(gameObject);
    }
    public void GetDamage(float amount)
    {
        currentKiaraHealth -= amount;
    }
}
