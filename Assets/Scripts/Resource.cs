using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : PickUpable
{
    public ResourceItem resourceItem;
    public override void Use()
    {
        base.Use();
        if (!string.IsNullOrEmpty(resourceItem.description))
        {
            Debug.Log(resourceItem.description);
        }
        
    }

    private void Start()
    {
        if (resourceItem.name.Equals("Wood"))
            number = 5;
    }
    public override string GetName()
    {
        return base.GetName() + resourceItem.name;
    }

    public override int GetMaxNumber()
    {
        return base.GetMaxNumber() + resourceItem.maxNumber;
    }

    public override Sprite GetSprite()
    {
        return resourceItem.icon;
    }

    /*public override void PickUp()
    {
        if (Inventory.instance.AddItem(this))
            Destroy(gameObject);
    }*/
}
