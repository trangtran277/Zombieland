using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : PickUpable
{
    public EquipmentItem equipmentItem;
    public override void Use()
    {
        base.Use();
        EquipmentManager.instance.Equip(this);
        Debug.Log(equipmentItem.name + "Equiped");
    }

    public override string GetName()
    {
        return base.GetName() + equipmentItem.name;
    }

    public override int GetMaxNumber()
    {
        return base.GetMaxNumber() + 1;
    }

    public override Sprite GetSprite()
    {
        return equipmentItem.icon;
    }

    /*public override void PickUp()
    {
        if (Inventory.instance.AddItem(this))
            Destroy(gameObject);
    }*/
}
