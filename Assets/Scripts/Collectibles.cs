using HutongGames.PlayMaker.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour, IInteractable
{
    public Item item;
    public int number;
    public void Interact()
    {
        PickUp();
    }
    public void PickUp()
    {
        Inventory inventory = Inventory.instance;
        int countBefore = inventory.items.Count;
        if (Inventory.instance.AddItem(this))
        {
            if (countBefore == inventory.items.Count)
            {
                Destroy(gameObject);
                return;
            }
            
            var components = GetComponents(typeof(Component));
            foreach (var comp in components)
            {
                if((comp is Collectibles) || (comp is Transform))
                {
                    continue;
                }
                Destroy(comp);
            }
            //transform.SetParent(GameObject.Find("GameManager").transform);
        }
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.RemoveItem(this);
    }

    public virtual void Use()
    {
        Debug.Log("Collectible used");
    }
}
