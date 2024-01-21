using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuraTowerScript : MonoBehaviour
{
    public TowerType gura;
    
    private Transform firePos;
    
    private float timeBtwAttacks;
    private float currentGuraHealth;
    private float guraHealth;
    
    private bool isInMeele = false;

    private float _meeleDamage;

    private Animator anim;

    private GameObject target;

    private void Start()
    {
        _meeleDamage = gura.meleeDamage;
        timeBtwAttacks = gura.timeBtwAtk;
        anim = GetComponent<Animator>();
        firePos = gameObject.transform.GetChild(0);
        guraHealth = gura.health;
        currentGuraHealth = guraHealth;
    }

    private void Update()
    {
        if (timeBtwAttacks <= 0 && isInMeele == false)
        {
            anim.Play("RangedAttack");
            Invoke("BulletInstantiate", 0.3f);
            timeBtwAttacks = gura.timeBtwAtk;
        }
        else if (timeBtwAttacks <= 0 && isInMeele == true)
        {
            anim.Play("MeleeAttack");
            target.GetComponent<EnemyController>()?.GetDamage(_meeleDamage);
            timeBtwAttacks = gura.timeBtwAtk;
        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }

        if (currentGuraHealth <= 0)
        {
            Destroy(this.gameObject);
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
        Instantiate(gura.waterballPref, firePos.transform.position, Quaternion.identity);
    }

    public void GetDamage(float amount)
    {
        currentGuraHealth -= amount;
    } 
}
