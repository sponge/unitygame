using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour {

    public abstract void UpdatePress(bool held);

    public abstract bool isFiring();
}