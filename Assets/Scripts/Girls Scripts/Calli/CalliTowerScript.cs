using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CalliTowerScript : MonoBehaviour
{
    public TowerType calli;
    private float timeBtwAttacks;
    private float currentCalliHealth;
    private float calliHealth;
    private float _damage;

    private Animator anim;

    private List<GameObject> enemiesInRange = new List<GameObject>();

    private void Start()
    {
        _damage = calli.damage;
        timeBtwAttacks = calli.timeBtwAtk;
        anim = GetComponent<Animator>();
        calliHealth = calli.health;
        currentCalliHealth = calliHealth;
    }

    private void Update()
    {
        if (timeBtwAttacks <= 0 && enemiesInRange.Any())
        {
            anim.Play("Attack");
            foreach (GameObject enemy in enemiesInRange)
            {
                enemy.gameObject.GetComponent<EnemyController>()?.GetDamage(_damage);
            }
            timeBtwAttacks = calli.timeBtwAtk;
        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }

        if (currentCalliHealth <= 0)
        {
            Destroy(this.GameObject());
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.gameObject);
        }
    }

    public void GetDamage(float amount)
    {
        currentCalliHealth -= amount;
    }
}
