using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Collectibles
{
    //public EquipmentItem equipmentItem;
    public override void Use()
    {
        //base.Use();
        EquipmentManager.instance.Equip(this);
    }

    /*public override string GetName()
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
    }*/
}
