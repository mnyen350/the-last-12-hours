using System;
using System.Collections;
using UnityEngine;


public class Enemy : Entity
{

    //private SpriteRenderer sr;
    protected Rigidbody2D rb;

    protected float _nextAttackTime;
    protected TimeSpan _attackSpeed;
    protected float _attackRange;
    protected float _chaseRange;
    protected bool _isChasing;

    public bool isPlayerInVision => Vector2.Distance(Player.Instance.position, this.rb.position) <= _vision;
    public bool isPlayerInChaseRange => Vector2.Distance(Player.Instance.position, this.rb.position) <= _chaseRange;
    public bool isPlayerInAttackRange => Vector2.Distance(Player.Instance.position, this.rb.position) <= _attackRange;
    public bool isAttackCooldown => _nextAttackTime > Time.time;

    protected void UpdateAttackCooldown()
    {
        // add cd
        _nextAttackTime = Time.time + (float)_attackSpeed.TotalSeconds;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isPlayerInVision && !_isChasing)
            _isChasing = true;

        if (_isChasing)
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
                Vector2 movement = (Player.Instance.position - rb.position).normalized;
                ApplyMovement(movement);
            }
            else // stop chasing
            {
                // chase range can be higher than vision range so that
                // once a monster has begun to chase the player it can chase further
                // than aggro vision
                // to avoid this behavior just set chase and vision to the same value
                _isChasing = false;
                StopMovement();
            }
        }

        if (!_isChasing)
            rb.velocity = Vector2.zero;
    }

    public override void ReceiveAttack(Entity source, int damage)
    {
        throw new NotImplementedException();
    }

    protected override void Attack()
    {
        // damage calculation
        int damage = _damage;

        // apply damage
        Player.Instance.ReceiveAttack(this, damage);
    }

    protected override void Walk()
    {

    }
}