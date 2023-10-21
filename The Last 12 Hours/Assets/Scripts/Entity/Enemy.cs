﻿using System;
using System.Collections;
using UnityEngine;


public class Enemy : Entity
{
    private float _nextAttackTime;

    [field: SerializeField]
    public float attackSpeed { get; protected set; }
    [field: SerializeField]
    public float attackRange { get; protected set; }
    [field: SerializeField]
    public float chaseRange { get; protected set; }
    public bool isChasing { get; protected set; }

    public override int attack => 1;

    public bool isPlayerInVision => Vector2.Distance(Player.Instance.position, this.rb.position) <= visionDistance;
    public bool isPlayerInChaseRange => Vector2.Distance(Player.Instance.position, this.rb.position) <= chaseRange;
    public bool isPlayerInAttackRange => Vector2.Distance(Player.Instance.position, this.rb.position) <= attackRange;
    public bool isAttackCooldown => _nextAttackTime > Time.time;

    protected override void Start()
    {
        health = maxHealth;
        this.OnDeath += Enemy_OnDeath;
        base.Start();
    }

    private void Enemy_OnDeath()
    {
        Debug.Log("Enemy is dead");
        this.ani?.SetTrigger("triggerDeath");
        DropReward();

        manager.DelayCallback(TimeSpan.FromSeconds(0.7), () =>
        {
            Destroy(this.gameObject);
        });
    }

    protected virtual void DropReward()
    {

    }

    protected void UpdateAttackCooldown()
    {
        // add cd
        _nextAttackTime = Time.time + (float)attackSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            isChasing = false;
            canMove = false;
            StopMovement();
        }

        if (isPlayerInVision && !isChasing)
            isChasing = true;

        if (isChasing)
        {
            if (isPlayerInAttackRange)
            {
                if (!isAttackCooldown)
                {
                    UpdateAttackCooldown();
                    Attack();
                }

                // stop moving because close enuogh to player to attack
                StopMovement();
            }
            else if (isPlayerInChaseRange)
            {
                Vector2 movement = (player.position - rb.position).normalized;
                ApplyMovement(movement);
            }
            else // stop chasing
            {
                // chase range can be higher than vision range so that
                // once a monster has begun to chase the player it can chase further
                // than aggro vision
                // to avoid this behavior just set chase and vision to the same value
                isChasing = false;
                StopMovement();
            }
        }
    }

    public override void ReceiveAttack(Entity source, int damage)
    {
        Debug.Log("Enemy receive damage");
        base.ReceiveAttack(source, damage);
    }

    protected virtual void Attack() => player.ReceiveAttack(this, this.attack);
    
}