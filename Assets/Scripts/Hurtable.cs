using UnityEngine;
using System;

public class Hurtable : MonoBehaviour {

    public int maxHealth;
    public int startingHealth;
    public int currentHealth;
    public float hurtCooldown;
    public bool invulnerable = true;
    public bool fadeOnInvulnerable;

    public event Action onHurt;
    public event Action onDeath;

    private float lastHurtTime;

    private SpriteRenderer sprite;

    void Start () {
        currentHealth = startingHealth;
        sprite = GetComponent<SpriteRenderer>();
	}

	void Update () {
        if (sprite && fadeOnInvulnerable)
        {
            sprite.color = new Color(1.0f, 1.0f, 1.0f, canTakeDamage() ? 1.0f : 0.5f);
        }
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
