using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiaraTowerScript : MonoBehaviour
{
    public TowerType kiara;
    private Transform firePos;
    public float dynamicTimeBtwAttack;
    private float timeBtwAttck;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        timeBtwAttck = kiara.timeBtwAtk;
        dynamicTimeBtwAttack = kiara.timeBtwAtk;
        firePos = gameObject.transform.GetChild(0);
    }

    private void Update()
    {
        if (timeBtwAttck <= 0)
        {
            anim.Play("Attack");
            Invoke("FireballInstantiate", 0.4f);
            timeBtwAttck = dynamicTimeBtwAttack;
        }
        else
        {
            timeBtwAttck -= Time.deltaTime;
        }
    }

    private void FireballInstantiate()
    {
        Instantiate(kiara.fireballPref, firePos.transform.position, Quaternion.identity);
    }
}
