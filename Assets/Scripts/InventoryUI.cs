using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    public Transform itemsParent;
    InventorySlot[] inventorySlots;
    public GameObject inventoryUI;
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        inventorySlots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
            

        }
    }
    
    public void ToggleInventory()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }
    void UpdateUI()
    {
        int i = 0;
        foreach(var item in inventory.items)
        {
            inventorySlots[i].AddItem(item);
            i++;
        }
        for(int j=i; j < inventorySlots.Length; j++)
        {
                inventorySlots[i].ClearSlot();
        }
    }
}
