using UnityEngine;
using System;

public class Hurtable : MonoBehaviour {

    public int maxHealth;
    public int startingHealth;
    public int currentHealth;
    public bool invulnerable;

    public event Action onHurt;
    public event Action onDeath;


    // Use this for initialization
    void Start () {
        currentHealth = startingHealth;
	}
	
	// Update is called once per frame
	void Update () {
        if (onHurt != null)
        {
            onHurt();
        }
    }

    public void Hurt(int amount)
    {
        if (amount < 0)
        {
            return;
        }

        currentHealth -= amount;

        if (currentHealth < 0)
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
