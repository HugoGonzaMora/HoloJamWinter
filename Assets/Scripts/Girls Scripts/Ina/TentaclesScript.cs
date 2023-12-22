using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentaclesScript : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        anim.Play("TentacleAttackAnim");
        Invoke("Destroy", 1.5f);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
