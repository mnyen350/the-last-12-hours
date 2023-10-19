using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

public class Rat : Entity
{

    //private SpriteRenderer sr;
    private Rigidbody2D rb;

    private float _canAttackTime;
    private float _attackIcd;
    private float _reach;

    // Start is called before the first frame update
    void Start()
    {
        _reach = 0.5f;
        _vision = 1.5f;
        _damage = 1;
        _attackIcd = 2500;
        Speed = 0.5f;
        canMove = true; 
        
        //sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var isPlayerInVision = Physics2D.OverlapCircleAll(transform.position, _vision)
            .Select(x => x.GetComponent<Player>())
            .Where(x => x != null)
            .Any();

        if (isPlayerInVision)
        {
            float d = Vector2.Distance(Player.Instance.position, rb.position);
            if (d < _reach)
            {
                // stop moving because close enuogh to player to attack
                rb.velocity = Vector2.zero;

                // is rat attack on ICD?
                if (_canAttackTime < Time.time)
                {
                    // add cd
                    _canAttackTime = Time.time + (float)TimeSpan.FromMilliseconds(_attackIcd).TotalSeconds;

                    // TO-DO: attack animation, etc..

                    // issue the attack
                    Attack();
                }
            }
            else
            {
                Vector2 movement = (Player.Instance.position - rb.position).normalized;
                rb.velocity = movement * Speed;
            }
        }
        else
        {
            //Debug.Log("Out of vision");
            rb.velocity = Vector2.zero;
        }
    }

    protected override void Attack() 
    { 
        Player.Instance.ReceiveDamange
    }

    protected override void Walk() 
    {
        
    }
}
