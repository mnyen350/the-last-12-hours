using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected int _maxHealth;

    [SerializeField]
    protected int _currentHealth;

    [SerializeField]
    protected int _damage; //flat damage for this class

    [SerializeField]
    protected float _vision; //is this pixels? 

    [SerializeField]
    protected float Speed;

    [SerializeField]
    protected bool canMove;

    protected abstract void Attack();
    protected abstract void Walk();

    public abstract void ReceiveAttack(Entity source, int damage);

}
