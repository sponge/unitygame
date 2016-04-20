using UnityEngine;
using System.Collections;

public class BaseWeapon : MonoBehaviour {
    public virtual void Start () {
	
	}

    public virtual void UpdatePress (bool held) {
	
	}

    public virtual bool isFiring()
    {
        return false;
    }
}
