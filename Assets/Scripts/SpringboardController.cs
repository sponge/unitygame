using Prime31;
using System.Collections;
using UnityEngine;

public class SpringboardController : MonoBehaviour {
    public float bounceHeight;
    public float cooldown;

    private BoxCollider2D col;
    private Animator anim;
    private RaycastHit2D[] hits = new RaycastHit2D[4];
    private float awakeTime;
    private Vector2 start;

    private void Start() {
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        start = new Vector2(col.bounds.min.x + 0.1f, col.bounds.max.y + 1);
    }

    private void Update() {
        if (Time.time < awakeTime) {
            return;
        }

        var hitCount = Physics2D.RaycastNonAlloc(start, Vector2.right, hits, col.bounds.size.x - 0.2f);
        for (var i = 0; i < hitCount; i++) {
            var charComp = hits[i].collider.gameObject.GetComponent<CharacterController2D>();
            if (charComp != null) {
                charComp.velocity.y = bounceHeight;
                anim.Play("springboard_pop");
                awakeTime = Time.time + cooldown;
            }
        }
    }
}