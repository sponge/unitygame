using UnityEngine;
using System.Collections;

public class CoinCollectable : BaseCollectable {

	void Start()
    {

	}
	
	void Update()
    {
	
	}

    public override void OnCollect(Inventory inventory)
    {
        inventory.items.coins += 1;
    }
}
