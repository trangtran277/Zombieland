using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/*public struct InventoryItem
{
    public Item item;
    public int number;
    public InventoryItem(Item newitem, int newnumber)
    {
        item = newitem;
        number = newnumber;
    }
}*/
public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
    #endregion
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public int space = 24;
    public List<PickUpable> items = new List<PickUpable>();
    public bool AddItem(PickUpable pickUpable)
    {
        if(pickUpable is Equipment)
        {
            //Debug.Log("Im equipment");
            if (items.Count >= space)
            {
                Debug.Log("Inventory full");
                return false;
            }
            items.Add(pickUpable);
            if(onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
        else
        {
            PickUpable itemInInventory = null;
            //if (items.Count > 0)
                //itemInInventory = items[0]; //= items.FindLast(i => i.GetName().Equals(pickUpable.GetName()));
            foreach (var item in items)
            {
                if (item.GetName().Equals(pickUpable.GetName()))
                {
                    itemInInventory = item;
                }                   
            }
            if (itemInInventory != null)
            {
               
                if(itemInInventory.number + pickUpable.number > pickUpable.GetMaxNumber())
                {
                    pickUpable.number = itemInInventory.number + pickUpable.number - pickUpable.GetMaxNumber();
                    itemInInventory.number = pickUpable.GetMaxNumber();
                    if (onItemChangedCallback != null)
                        onItemChangedCallback.Invoke();
                    if (items.Count >= space)
                    {
                        Debug.Log("Inventory full");
                        return false;
                    }
                    items.Add(pickUpable);
                    if (onItemChangedCallback != null)
                        onItemChangedCallback.Invoke();
                }
                else
                {
                    itemInInventory.number += pickUpable.number;
                    if (onItemChangedCallback != null)
                        onItemChangedCallback.Invoke();
                }
            }
            else
            {
                if (items.Count >= space)
                {
                    Debug.Log("Inventory full");
                    return false;
                }
                items.Add(pickUpable);
                if (onItemChangedCallback != null)
                    onItemChangedCallback.Invoke();
            }
        }
        //foreach (var item in items)
          //  Debug.Log(item.GetName());
        return true;
           
    }

    public void ItemChanged(PickUpable pickUpable, float newNumber)
    {
        //PickUpable itemInInventory = items.Find(i => i.GetName() == pickUpable.GetName());
        //itemInInventory.number = newNumber;
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void RemoveItem(PickUpable pickUpable)
    {
        items.Remove(pickUpable);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
