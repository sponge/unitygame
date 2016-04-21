using UnityEngine;
using System;

public class Hurtable : MonoBehaviour {

    public int maxHealth;
    public int startingHealth;
    public int currentHealth;
    public float damageCooldownTime;
    public bool invulnerable = true;
    public bool fadeOnInvulnerable;

    public event Action onHurt;
    public event Action onDeath;

    private float lastHurtTime;

    private SpriteRenderer sprite;

    void Start ()
    {
        currentHealth = startingHealth;
        sprite = GetComponent<SpriteRenderer>();
	}

	void Update ()
    {
        if (sprite && fadeOnInvulnerable)
        {
            sprite.color = new Color(1.0f, 1.0f, 1.0f, isOnDamageCooldown() ? 0.5f : 1.0f);
        }
    }

    public bool isOnDamageCooldown()
    {
        return Time.time < lastHurtTime;
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
        lastHurtTime = Time.time + damageCooldownTime;

        var sprite = gameObject.GetComponent<Renderer>();
        if (sprite)
        {
            var dmgObj = (GameObject)Instantiate(Resources.Load("DamageNumber"));
            dmgObj.transform.localPosition = new Vector3(sprite.transform.position.x, sprite.bounds.max.y + 5, -50);
            var dmg = dmgObj.GetComponent<DamageNumber>();
            dmg.value = amount.ToString();
            dmg.Trigger();
        }

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
