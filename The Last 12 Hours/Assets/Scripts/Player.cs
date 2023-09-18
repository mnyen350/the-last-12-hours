using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : Entity
{
    // Start is called before the first frame update
    void Awake()
    {

        // This is for not destroying the player when the scene is changed
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        // Do not overload the main method
        UpdateMove();
    }

    private void UpdateMove()
    {
        if (canMove == false) return;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(x, y, 0);
        transform.Translate(movement * Speed * Time.deltaTime);
    }

    protected override void Attack()
    {
        Debug.Log("Attack Player");
    }

    protected override void Walk()
    {
        Debug.Log("Walk Player");
    }

    protected void Heal(int healAmount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + healAmount, 0, _maxHealth);
        Debug.Log("Player healed");
    }
}
