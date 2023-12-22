using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InaTowerScript : MonoBehaviour
{
    public TowerType ina;
    private float timeBtwAttacks;

    private Animator anim;

    private Vector3 enemyPos;

    private void Start()
    {
        timeBtwAttacks = ina.timeBtwAtk;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 rayOrigin = transform.position;

        Vector2 rayDirection = Vector2.right;

        float rayLength = 50f;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                enemyPos = hit.transform.position;
            }
        }
        
        ////////////////////////////////////////////////////////////
        
        if (timeBtwAttacks <= 0 && hit.collider.CompareTag("Enemy"))
        {
            anim.Play("RangedAttack");
            Invoke("BulletInstantiate", 0.3f);
            timeBtwAttacks = ina.timeBtwAtk;
        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }
    }

    private void BulletInstantiate()
    {
        Instantiate(ina.tentaclePref, enemyPos, Quaternion.identity);
    }
}
