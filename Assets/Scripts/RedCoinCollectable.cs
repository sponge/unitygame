public class RedCoinCollectable : BaseCollectable {

    private void Start() {
    }

    private void Update() {
    }

    public override void OnCollect(Inventory inventory) {
        inventory.items.redCoins += 1;
    }
}