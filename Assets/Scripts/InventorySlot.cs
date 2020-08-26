using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class InventorySlot : MonoBehaviour
{
    public Collectibles item;
    public Image icon;
    //public Button removeButton;
    public GameObject numberPanel;
    //public Image itemDesIcon;
    //public Text itemDesName;
    //public Text itemDes;
    //public Button useButton;
    //public Button discardButton;
    //public Text numberText;
    public void AddItem(Collectibles collectibles)
    {
        item = collectibles;
        icon.sprite = collectibles.item.icon;
        icon.preserveAspect = true;
        icon.enabled = true;
        //removeButton.interactable = true;
        
        /*if(collectibles is Consumable)
        {
            numberPanel.GetComponent<Image>().enabled = true;
            numberPanel.GetComponentInChildren<Text>().text = collectibles.number.ToString();
            numberPanel.GetComponentInChildren<Text>().enabled = true;
        }*/    
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        //removeButton.interactable = false;
        numberPanel.GetComponent<Image>().enabled = false;
        numberPanel.GetComponentInChildren<Text>().enabled = false;
    }

    /*public void OnDiscardButton()
    {
        Inventory.instance.RemoveItem(item);
        useButton.interactable = false;
        discardButton.interactable = false;
    }

    public void UseItem()
    {
        if(item != null)
        {
            item.Use();
            if(item == null)
            {
                useButton.interactable = false;
                discardButton.interactable = false;
            }
        }

    }*/

    public void OnItemPressed()
    {
        GetComponentInParent<InventoryUI>().OnInventorySlotPressed(this); 
    }
}
