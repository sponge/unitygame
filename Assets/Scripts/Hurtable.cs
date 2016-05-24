using System;
using UnityEngine;

public class Hurtable : MonoBehaviour {
    public int maxHealth;
    public int startingHealth;
    public int currentHealth;
    public float damageCooldownTime;
    public bool invulnerable = true;
    public bool fadeOnInvulnerable;

    public event Func<int, Vector3, bool> canHurt;

    public event Action<int, Vector3> onHurt;

    public event Action onDeath;

    private float lastHurtTime;

    private SpriteRenderer sprite;

    private void Start() {
        currentHealth = startingHealth;
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (sprite && fadeOnInvulnerable) {
            sprite.color = new Color(1.0f, 1.0f, 1.0f, isOnDamageCooldown() ? 0.5f : 1.0f);
        }
    }

    public bool isOnDamageCooldown() {
        return Time.time < lastHurtTime;
    }

    public bool canTakeDamage(int amount, Vector3 dir) {
        if (invulnerable) {
            return false;
        }

        if (Time.time < lastHurtTime) {
            return false;
        }

        if (canHurt != null && !canHurt(amount, dir)) {
            return false;
        }

        return true;
    }

    private void CheckDeath() {
        if (currentHealth <= 0) {
            if (onDeath != null) {
                onDeath();
            }
            else {
                Destroy(this.gameObject);
            }
        }
    }

    public void InstaGib() {
        currentHealth = 0;
        CheckDeath();
    }

    public bool Hurt(int amount, Vector3 dir) {
        dir = Vector3.Normalize(dir - transform.position);

        if (amount < 0 || !canTakeDamage(amount, dir)) {
            return false;
        }

        currentHealth -= amount;
        lastHurtTime = Time.time + damageCooldownTime;

        var sprite = gameObject.GetComponent<Renderer>();
        if (sprite) {
            var dmgObj = (GameObject)Instantiate(Resources.Load("DamageNumber"));
            dmgObj.transform.localPosition = new Vector3(sprite.transform.position.x, sprite.bounds.max.y + 5, -50);
            var dmg = dmgObj.GetComponent<DamageNumber>();
            dmg.value = amount.ToString();
            dmg.Trigger();
        }

        if (onHurt != null) {
            onHurt(amount, dir);
        }

        CheckDeath();
        return true;
    }
}