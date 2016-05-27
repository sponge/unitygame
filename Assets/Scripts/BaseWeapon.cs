using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour {
    public Vector3 handOffset;

    public abstract void UpdatePress(bool held);

    public abstract void Fire();

    public abstract bool isFiring();
}