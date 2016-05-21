using UnityEngine;

public abstract class BaseCollectable : MonoBehaviour
{
    public bool dontDestroyOnCollect;

    public void Collect(Inventory inventory)
    {
        OnCollect(inventory);

        if (!dontDestroyOnCollect)
        {
            Destroy(gameObject);
        }
    }

    public abstract void OnCollect(Inventory inventory);

}
