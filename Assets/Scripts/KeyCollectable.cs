using UnityEngine;
using System.Collections;

public class KeyCollectable : BaseCollectable
{

    public bool redKey;
    public bool greenKey;
    public bool blueKey;
    public bool yellowKey;

    public override void OnCollect(Inventory inventory)
    {
        if (redKey)
        {
            inventory.items.redKey = true;
        }

        if (greenKey)
        {
            inventory.items.greenKey = true;
        }

        if (blueKey)
        {
            inventory.items.blueKey = true;
        }

        if (yellowKey)
        {
            inventory.items.yellowKey = true;
        }
    }
}
