using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : PickUpable
{
    public ConsumableItem consumableItem;

    private void Start()
    {
        if (consumableItem.name.Equals("Berries"))
            number = (int)Random.Range(3, 5);
    }
    public override void Use()
    {
        base.Use();
        number--;
        if (number <= 0)
            RemoveFromInventory();
        else
            Inventory.instance.ItemChanged(this, number);
        consumableItem.TakeEffect();
    }

    public override string GetName()
    {
        return base.GetName() + consumableItem.name;
    }

    public override int GetMaxNumber()
    {
        return base.GetMaxNumber() + consumableItem.maxNumber;
    }

    public override Sprite GetSprite()
    {
        return consumableItem.icon;
    }

    /*public override void PickUp()
    {
        if (Inventory.instance.AddItem(this))
            Destroy(gameObject);
    }*/
}
