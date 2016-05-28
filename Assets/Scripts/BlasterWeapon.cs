using UnityEngine;

public class BlasterWeapon : BaseWeapon {
    public int damage;
    public int burstAmount;
    public float burstSpeed;
    public BulletController projectile;

    private bool attackHeld;
    private float attackTime;
    private int shotsTaken = 0;
    private bool burstActive = false;

    public void Start() {
    }

    override public void UpdatePress(bool attackPress) {
        if (!burstActive) {
            return;
        }

        if (attackHeld && !attackPress) {
            attackHeld = false;
        }

        if (attackPress && !attackHeld) {
            Fire();
        }
    }

    override public bool isFiring() {
        return burstActive;
    }

    override public void Fire() {
        attackTime = Time.time;
        attackHeld = true;
        burstActive = true;
    }

    public void Update() {
        if (!(burstActive && Time.time > attackTime + shotsTaken * burstSpeed)) {
            return;
        }

        var bullet = Instantiate(projectile);
        bullet.transform.localPosition = gameObject.transform.position + handOffset;
        bullet.damage = damage;

        shotsTaken++;
        if (shotsTaken >= burstAmount) {
            burstActive = false;
            shotsTaken = 0;
        }
    }
}