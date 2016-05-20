using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

    [System.Serializable]
    public struct Items
    {
        public int coins;
        public bool redKey;
    }

    public Items items;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
