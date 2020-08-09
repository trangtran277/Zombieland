using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


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
    public int space = 12;
    public List<Collectibles> items = new List<Collectibles>();

    public bool AddItem(Collectibles collectibles)
    {
        if(collectibles is Equipment || collectibles is MissionItem)
        {
            if (items.Count >= space)
            {
                Debug.Log("Inventory full");
                return false;
            }
            items.Add(collectibles);
            Debug.Log("Item added");
            if(onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
                Debug.Log("callback");
            }
                
        }
        else
        {
            Collectibles itemInInventory = null;
            itemInInventory = items.FindLast(i => i.item.name.Equals(collectibles.item.name));
            if (itemInInventory != null)
            {
                int maxNumberInSlot = itemInInventory.item.maxNumberPerSlot;
                if (itemInInventory.number + collectibles.number > maxNumberInSlot)
                {
                    collectibles.number = itemInInventory.number + collectibles.number - maxNumberInSlot;
                    itemInInventory.number = maxNumberInSlot;
                    if (onItemChangedCallback != null)
                        onItemChangedCallback.Invoke();
                    if (items.Count >= space)
                    {
                        Debug.Log("Inventory full");
                        return false;
                    }
                    items.Add(collectibles);
                    if (onItemChangedCallback != null)
                        onItemChangedCallback.Invoke();
                }
                else
                {
                    itemInInventory.number += collectibles.number;
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
                items.Add(collectibles);
                if (onItemChangedCallback != null)
                    onItemChangedCallback.Invoke();
            }
        }
        return true;
           
    }

    public void ItemChanged()
    {
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void RemoveItem(Collectibles collectibles)
    {
        items.Remove(collectibles);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        if(collectibles != EquipmentManager.instance.currentEquipment[3])
            Destroy(collectibles.gameObject);
    }
}
