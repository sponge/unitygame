using UnityEngine;

public class DoorTrigger : BaseTrigger {
    public bool redKeyOpens;
    public bool greenKeyOpens;
    public bool blueKeyOpens;
    public bool yellowKeyOpens;

    public override void OnTrigger(GameObject triggerObj) {
        if (!redKeyOpens && !greenKeyOpens && !blueKeyOpens && !yellowKeyOpens) {
            Destroy(gameObject);
        }

        var inventory = triggerObj.GetComponent<Inventory>();

        if (inventory == null) {
            Debug.Log("couldn't find an inventory");
            return;
        }

        if (
            (redKeyOpens && inventory.items.redKey) ||
            (greenKeyOpens && inventory.items.greenKey) ||
            (blueKeyOpens && inventory.items.blueKey) ||
            (yellowKeyOpens && inventory.items.yellowKey)
            ) {
            Destroy(gameObject);
        }
    }
}