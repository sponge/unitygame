﻿using UnityEngine;
using System.Collections;

public class CoinCollectable : BaseCollectable {

    public override void OnCollect(Inventory inventory)
    {
        inventory.items.coins += 1;
    }
}
