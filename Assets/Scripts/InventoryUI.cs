using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    public Transform itemsParent;
    InventorySlot[] inventorySlots;
    public Image itemDesIcon;
    public Text itemDesName;
    public Text itemDes;
    public Button useButton;
    public Button discardButton;
    public GameObject notesUI;
    public GameObject objectivesUI;
    public InventorySlot equipmentSlot;
    public GameObject inventoryUI;
    InventorySlot currentSelected = null;
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateInventoryUI;
        EquipmentManager.instance.onEquipmentChanged += UpdateEquipmentSlot;
        inventorySlots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    public void ToggleInventory()
    {
        if (currentSelected != null)
        {
            currentSelected.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
            currentSelected = null;
            UpdateSelectedItem(null);
        }
            
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }
    void UpdateInventoryUI()
    {
        int i = 0;
        foreach(var item in inventory.items)
        {
            inventorySlots[i].ClearSlot();
            inventorySlots[i].AddItem(item);
            i++;
        }
        for(int j = i; j < inventorySlots.Length; j++)
        {
            if (j == i)
            {
                if(inventorySlots[j].item != null)
                {
                    currentSelected.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
                    
                    currentSelected = null;
                    UpdateSelectedItem(null);
                }
            }
            inventorySlots[i].ClearSlot();
        }
    }

    public void UpdateEquipmentSlot(Equipment newEquip, Equipment oldEquip)
    {
        if (newEquip == null)
            equipmentSlot.ClearSlot();
        else
            equipmentSlot.AddItem(newEquip);
    }
    public void OnInventorySlotPressed(InventorySlot inventorySlot)
    {
        if(currentSelected != null)
            currentSelected.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
        inventorySlot.GetComponentInChildren<Image>().color = new Color32(162, 157, 157, 255);
        currentSelected = inventorySlot;
        Collectibles selectedItem = currentSelected.item;
        UpdateSelectedItem(selectedItem);

    }

    public void UpdateSelectedItem(Collectibles item)
    {
        if (item != null)
        {
            if(item is Equipment)
            {
                if (item == EquipmentManager.instance.currentEquipment[3])
                    useButton.GetComponentInChildren<Text>().text = "Unequip";
                else
                    useButton.GetComponentInChildren<Text>().text = "Equip";
            }
            else
                useButton.GetComponentInChildren<Text>().text = "Use";
            useButton.interactable = true;
            discardButton.interactable = true;
            itemDesIcon.sprite = item.item.icon;
            itemDesName.text = item.item.name;
            itemDes.text = item.item.description;
            itemDesIcon.enabled = true;
            itemDesName.enabled = true;
            itemDes.enabled = true;
        }
        else
        {
            useButton.interactable = false;
            discardButton.interactable = false;
            itemDesIcon.sprite = null;
            itemDesName.text = null;
            itemDes.text = null;
            itemDesIcon.enabled = false;
            itemDesName.enabled = false;
            itemDes.enabled = false;
        }
    }

    public void OnButtonUse()
    {
        if (useButton.GetComponentInChildren<Text>().text.Equals("Unequip"))
        {
            Debug.Log("unequip");
            EquipmentManager.instance.Unequip(3);
            currentSelected.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
            currentSelected = null;
            UpdateSelectedItem(null);
        }
        else
            currentSelected.item.Use();
    }

    public void OnButtonDiscard()
    {
        EquipmentManager equipmentManager = EquipmentManager.instance;
        if (currentSelected.item == equipmentManager.currentEquipment[3])
        {
            equipmentManager.currentEquipment[3] = null;
            Destroy(currentSelected.item.gameObject);
            currentSelected.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
            currentSelected = null;
            UpdateSelectedItem(null);
            UpdateEquipmentSlot(null, null);
        }
        else
            currentSelected.item.RemoveFromInventory();
    }

    public void ToggleNotesUI()
    {
        notesUI.SetActive(!notesUI.activeSelf);
    }

    public void ToggleObjectivesUI()
    {
        objectivesUI.SetActive(!objectivesUI.activeSelf);
    }
}
