using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

public class Rat : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        attackRange = 0.5f;
        visionDistance = 1.5f;
        chaseRange = 1.5f;
        attackSpeed = TimeSpan.FromMilliseconds(2500);
        speed = 0.5f;
        canMove = true;
        base.Start();
    }

    protected override void DropLoot()
    {
        // example of dropping an item or something on death...
        Instantiate(manager.Prefabs.BandagePrefab, this.transform.position, Quaternion.identity);
    }

    protected override void Attack()
    {
        ani?.SetBool("isBite", true);
        base.Attack();
    }
}
