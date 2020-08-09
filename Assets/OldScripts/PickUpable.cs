using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpable : Interactable
{
    public float number;

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    public void PickUp()
    {
        int countBefore = Inventory.instance.items.Count;
        /*if (Inventory.instance.AddItem(this))
        {
            if (countBefore == Inventory.instance.items.Count)
            {
                Destroy(gameObject);
                return;
            }
            
            var components = GetComponents(typeof(Component));
            foreach (var comp in components)
            {
                if(!(comp is PickUpable) && !(comp is Transform))
                {
                    Destroy(comp);
                }
            }
            transform.SetParent(GameObject.Find("GameManager").transform);
        }*/
    }

    public void RemoveFromInventory()
    {
        //Inventory.instance.RemoveItem(this);
    }

    public virtual void Use()
    {
        Debug.Log("Using pickup");
    }

    public virtual string GetName()
    {
        return "";
    }

    public virtual int GetMaxNumber()
    {
        return 0;
    }
    
    public virtual Sprite GetSprite()
    {
        return null;
    }
}
