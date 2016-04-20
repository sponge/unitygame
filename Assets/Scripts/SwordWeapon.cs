﻿using UnityEngine;
using System.Collections;

public class SwordWeapon : BaseWeapon
{

    public Vector3 handOffset;
    public float length;

    override public void Start()
    {
        isReady = true;
    }

    override public void UpdatePress(bool attackPress)
    {
        if (attackHeld && !attackPress && isReady)
        {
            attackHeld = false;
        }

        if (attackPress && !attackHeld && Time.time > attackTime)
        {
            Fire();
        }
    }

    override public bool isFiring()
    {
        return Time.time < attackTime;
    }

    public void Fire()
    {
        attackTime = Time.time + 0.5f;
        attackHeld = true;
    }

    public void Update()
    {
        if (Time.time < attackTime)
        {
            var dir = new Vector3(1, 0, 0);
            Debug.DrawRay(transform.position + handOffset, dir * length * transform.localScale.x, Color.red);

            var hitList = Physics2D.RaycastAll(transform.position + handOffset, dir, length * transform.localScale.x);
            foreach (var hit in hitList)
            {
                if (this == hit.collider)
                {
                    continue;
                }

                var hurt = hit.collider.gameObject.GetComponent<Hurtable>();
                if (hurt)
                {
                    hurt.Hurt(1);
                }
            }
        }
    }
}
