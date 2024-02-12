using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InaTowerScript : MonoBehaviour
{
    public TowerType ina;
    private float timeBtwAttacks;

    private Animator anim;

    private Vector3 enemyPos;
    private float currentInaHealth;
    private float inaHealth;
    private int layerMask;
    private float rayLength;

    private void Start()
    {
        timeBtwAttacks = ina.timeBtwAtk;
        anim = GetComponent<Animator>();
        inaHealth = ina.health;
        currentInaHealth = inaHealth;
        
        layerMask = LayerMask.GetMask("Default");
        rayLength = 50f;
    }

    private void Update()
    {
        Vector2 rayOrigin = transform.position;

        Vector2 rayDirection = Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, layerMask);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
                enemyPos = hit.transform.position;
        }
        
        if (timeBtwAttacks <= 0 && hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Invoke("BulletInstantiate", 0.3f);
                timeBtwAttacks = ina.timeBtwAtk;
            }
        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }

        if (currentInaHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void BulletInstantiate()
    {
        Instantiate(ina.tentaclePref, enemyPos, Quaternion.identity);
    }

    public void GetDamage(float amount)
    {
        currentInaHealth -= amount;
    }
}
