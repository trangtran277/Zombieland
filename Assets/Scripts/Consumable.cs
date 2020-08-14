using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Consumable : Collectibles
{
    //public ConsumableItem consumableItem;

    /*private void Start()
    {
        if (consumableItem.name.Equals("Berries"))
            number = (int)Random.Range(3, 5);
    }*/
    public int healAmount = 30; 
    public override void Use()
    {
        //base.Use();
        number--;
        if (number <= 0)
            RemoveFromInventory();
        else
            Inventory.instance.ItemChanged();
        TakeEffect();
    }

    private void TakeEffect()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>().health += healAmount;
    }

    /*public override string GetName()
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
    }*/
}
