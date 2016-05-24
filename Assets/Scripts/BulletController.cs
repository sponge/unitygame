﻿using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

    public Vector3 speed;
    public Vector2 size;
    public int damage;

    private int explodeAnim = Animator.StringToHash("blaster_explode");
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
	}
	
	void Update()
    {
        var distance = Vector2.Distance(Vector2.zero, speed * Time.deltaTime);
        var hit = Physics2D.BoxCast(transform.position, size, 0, speed, distance);

        if (hit.collider != null)
        {
            var hurtable = hit.collider.gameObject.GetComponent<Hurtable>();
            if (hurtable != null)
            {
                hurtable.Hurt(damage, transform.position);
            }

            animator.Play(explodeAnim);
            Destroy(gameObject, 1.0f);
            enabled = false;
        } else
        {
            transform.position += speed * Time.deltaTime;
        }
	}
}
