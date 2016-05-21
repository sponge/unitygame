using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

    [System.Serializable]
    public struct Items
    {
        public int xp;
        public int maxHealth;
        public int strength;

        public int coins;
        public int redCoins;

        public bool redKey;
        public bool blueKey;
        public bool greenKey;
        public bool yellowKey;
    }

    public Items items;

    public void ResetTemporaryItems()
    {
        items.redCoins = 0;
        items.redKey = false;
        items.blueKey = false;
        items.greenKey = false;
        items.yellowKey = false;
    }
}
