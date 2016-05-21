using UnityEngine;

public abstract class BaseTrigger : MonoBehaviour
{

    public void Trigger(GameObject triggerObj)
    {
        OnTrigger(triggerObj);
    }

    public abstract void OnTrigger(GameObject triggerObj);

}
