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

    private float _bleedDamage;
    
    private int layerMask;
    private float rayLength;
    
    private bool isInMeele = false;

    private float _meeleDamage;

    private Animator anim;

    private GameObject target;

    private IEnumerator BleedCoroutine;

    private void Start()
    {
        _meeleDamage = gura.meleeDamage;
        timeBtwAttacks = gura.timeBtwAtk;
        anim = GetComponent<Animator>();
        firePos = gameObject.transform.GetChild(0);
        guraHealth = gura.health;
        currentGuraHealth = guraHealth;
        _bleedDamage = gura.bleedingDamage;
        
        layerMask = LayerMask.GetMask("Default");
        rayLength = 50f;
    }

    private void Update()
    {
        Vector2 rayOrigin = transform.position;

        Vector2 rayDirection = Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, layerMask);
        if (hit.collider != null && hit.collider.CompareTag("Enemy") && timeBtwAttacks <= 0 && isInMeele == false)
        {
            anim.Play("RangedAttack");
            Invoke("BulletInstantiate", 0.3f);
            timeBtwAttacks = gura.timeBtwAtk;
        }
        else if (timeBtwAttacks <= 0 && isInMeele == true)
        {
            anim.Play("MeleeAttack");
            target.GetComponent<EnemyController>()?.GetDamage(_meeleDamage);
            if (target.activeInHierarchy)
            {
                BleedCoroutine = target.GetComponent<EnemyController>()?.GetPassiveDamage(_bleedDamage);
                target.GetComponent<EnemyController>()?.StopCoroutine(BleedCoroutine);
    
                target.GetComponent<EnemyController>()?.StartCoroutine(BleedCoroutine);
            }
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
