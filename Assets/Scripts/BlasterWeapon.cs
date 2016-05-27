using UnityEngine;

public class BlasterWeapon : BaseWeapon {
    public int damage;
    public int burstAmount;
    public float burstSpeed;
    public GameObject projectile;

    private bool isReady;
    private bool attackHeld;
    private float attackTime;
    private int shotsTaken = 0;
    private bool burstActive = false;

    public void Start() {
        isReady = true;
    }

    override public void UpdatePress(bool attackPress) {
        if (attackHeld && !attackPress && isReady) {
            attackHeld = false;
        }

        if (attackPress && !attackHeld && Time.time > attackTime) {
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
        if (burstActive && Time.time > attackTime + (shotsTaken * burstSpeed)) {
            var spawnInstance = Instantiate(projectile);
            spawnInstance.transform.localPosition = gameObject.transform.position + handOffset;

            var bullet = spawnInstance.GetComponent<BulletController>();
            bullet.damage = damage;

            shotsTaken++;
            if (shotsTaken >= burstAmount) {
                burstActive = false;
                shotsTaken = 0;
            }
        }
    }
}