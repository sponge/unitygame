using UnityEngine;
using System.Collections;

public class BaseWeapon : MonoBehaviour {

    protected bool isReady;
    protected bool attackHeld;
    protected float attackTime;

    public virtual void Start () {
	
	}

    public virtual void UpdatePress (bool held) {
	
	}

    public virtual bool isFiring()
    {
        return false;
    }
}
