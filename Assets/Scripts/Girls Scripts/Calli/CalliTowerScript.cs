using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalliTowerScript : MonoBehaviour
{
    public TowerType calli;
    private float timeBtwAttacks;
    private bool isEnemyInZone = false;
    private float currentCalliHealth;
    private float calliHealth;

    private Animator anim;

    private void Start()
    {
        timeBtwAttacks = calli.timeBtwAtk;
        anim = GetComponent<Animator>();
        calliHealth = calli.health;
        currentCalliHealth = calliHealth;
    }

    private void Update()
    {
        if (timeBtwAttacks <= 0 && isEnemyInZone == true)
        {
            anim.Play("Attack");
            timeBtwAttacks = calli.timeBtwAtk;
        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            isEnemyInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            isEnemyInZone = false;
        }
    }

    public void GetDamage(float amount)
    {
        calliHealth -= amount;
    }
}
