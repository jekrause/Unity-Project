using UnityEngine;

public abstract class Item : MonoBehaviour{

    public abstract int GetMaxStackSize();

    public abstract void InteractWithItem();

}
