using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
    Inventory inventory;
    public Transform itemsParent;
    InventorySlot[] inventorySlots;
    public GameObject itemDesUI;
    public Image itemDesIcon;
    public Text itemDesName;
    public Text itemDes;
    public Button useButton;
    public Button discardButton;
    //public GameObject notesUI;
    //public GameObject objectivesUI;
    //public GameObject notesTitle;
    //public GameObject objectivesTitle;
    public GameObject interactionDes;
    public GameObject interactionTitle;
    public InventorySlot equipmentSlot;
    public GameObject inventoryUI;
    InventorySlot currentSelected = null;
    public AudioSource[] audios;
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateInventoryUI;
        EquipmentManager.instance.onEquipmentChanged += UpdateEquipmentSlot;
        inventorySlots = itemsParent.GetComponentsInChildren<InventorySlot>();
        audios = GetComponents<AudioSource>();
    }

    public void ToggleInventory(int type = 0)
    {
        if (currentSelected != null)
        {
            currentSelected.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
            currentSelected = null;
            UpdateSelectedItem(null);
        }
        //notesUI.SetActive(false);
        //objectivesUI.SetActive(false);
        interactionDes.SetActive(false);
        interactionTitle.SetActive(false);
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        if(type == 1)
        {
            //notesTitle.SetActive(false);
            //objectivesTitle.SetActive(false);
            interactionDes.SetActive(true);
            interactionTitle.SetActive(true);
        }
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
        {
            currentSelected.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
        }    
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
            if(item is MissionItem)
                discardButton.interactable = false;
            else
                discardButton.interactable = true;
            itemDesUI.SetActive(true);
            itemDesIcon.sprite = item.item.icon;
            itemDesName.text = item.item.name;
            itemDes.text = item.item.description;
            //itemDesIcon.enabled = true;
            //itemDesName.enabled = true;
            //itemDes.enabled = true;
        }
        else
        {
            itemDesUI.SetActive(false);
            useButton.GetComponentInChildren<Text>().text = "Use";
            useButton.interactable = false;
            discardButton.interactable = false;
            /*itemDesIcon.sprite = null;
            itemDesName.text = null;
            itemDes.text = null;
            itemDesIcon.enabled = false;
            itemDesName.enabled = false;
            itemDes.enabled = false;*/
        }
    }

    public void OnButtonUse()
    {
        if (currentSelected.item is Consumable)
            audios[0].Play();
        if (currentSelected.item is Equipment)
            audios[1].Play();
        if (useButton.GetComponentInChildren<Text>().text.Equals("Unequip"))
        {
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
}
