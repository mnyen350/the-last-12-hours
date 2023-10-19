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
        _attackRange = 0.5f;
        _vision = 1.5f;
        _chaseRange = 1.5f;
        _damage = 1;
        _attackSpeed = TimeSpan.FromMilliseconds(2500);
        Speed = 0.5f;
        canMove = true;
        base.Start();
    }

}
