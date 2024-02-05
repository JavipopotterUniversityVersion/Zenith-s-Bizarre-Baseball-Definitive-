using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    Item selectedItem;
    Transform itemHolder;

    [SerializeField] LayerMask targetLayer;

    float throwForce = 10f;
    float throwAngularForce = 10f;

    void Use()
    {
        selectedItem.Use();
    }

    void PickUp(Item item)
    {
        if(item.canBePickedUp)
        {
            if(selectedItem != null) 
            {
                selectedItem.Drop();
            }

            selectedItem = item;
            item.PickUp(itemHolder, targetLayer);
        }
    }

    

    void Throw(Vector2 direction)
    {
        if(selectedItem != null) 
        {
            selectedItem.Throw(direction * throwForce , throwAngularForce);
        }
    }
}