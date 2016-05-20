using UnityEngine;
using System.Collections;

public class RedCoinCollectable : BaseCollectable
{

    void Start()
    {

    }

    void Update()
    {

    }

    public override void OnCollect(Inventory inventory)
    {
        inventory.items.redCoins += 1;
    }
}
