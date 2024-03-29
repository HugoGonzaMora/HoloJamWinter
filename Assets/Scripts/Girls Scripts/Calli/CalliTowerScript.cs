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
            int randNum = Random.Range(1, 101);
            if (randNum == 3)
            {
                enemiesInRange[Random.Range(0, enemiesInRange.Count)].gameObject.GetComponent<EnemyController>()?.GetDamage(999);
            }
            anim.Play("Attack");
            foreach (GameObject enemy in enemiesInRange.ToList())
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
            Destroy(this.gameObject);
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
