using UnityEngine;
using System;

public class Hurtable : MonoBehaviour {

    public int maxHealth;
    public int startingHealth;
    public int currentHealth;
    public float hurtCooldown;
    public bool invulnerable;

    public event Action onHurt;
    public event Action onDeath;

    private float lastHurtTime;

    void Start () {
        currentHealth = startingHealth;
	}

	void Update () {

    }

    public bool canTakeDamage()
    {
        if (invulnerable)
        {
            return false;
        }

        if (Time.time < lastHurtTime)
        {
            return false;
        }

        return true;
    }

    public void Hurt(int amount)
    {
        if (amount < 0 || !canTakeDamage())
        {
            return;
        }

        currentHealth -= amount;
        lastHurtTime = Time.time + hurtCooldown;

        if (onHurt != null)
        {
            onHurt();
        }

        if (currentHealth <= 0)
        {
            if (onDeath != null)
            {
                onDeath();
            } else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
