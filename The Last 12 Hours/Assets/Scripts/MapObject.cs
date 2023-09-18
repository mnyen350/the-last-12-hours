using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map : MonoBehaviour
{
    [SerializeField]
    protected int _maxHealth;

    protected int _currentHealth;

    [SerializeField]
    protected bool canCollide = false;

    public virtual void ReceiveDamange(int damage)
    {
        this._currentHealth = Mathf.Clamp(this._currentHealth - damage, 0, _maxHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
