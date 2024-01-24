using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentaclesScript : MonoBehaviour
{
    private Animator anim;
    public TowerType inaTowerType;

    private float _damage;

    private BoxCollider2D boxCol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        _damage = inaTowerType.damage;
        anim.Play("TentacleAttackAnim");
        Invoke("Destroy", 1.5f);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>()?.GetDamage(_damage);
            boxCol.enabled = false;
        }
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
