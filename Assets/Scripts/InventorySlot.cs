using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class InventorySlot : MonoBehaviour
{
    PickUpable item;
    public Image icon;
    public Button removeButton;
    public GameObject numberPanel;
    //public Text numberText;
    public void AddItem(PickUpable pickUpable)
    {
        item = pickUpable;
        icon.sprite = pickUpable.GetSprite();
        icon.preserveAspect = true;
        icon.enabled = true;
        removeButton.interactable = true;
        numberPanel.GetComponent<Image>().enabled = true;
        
        numberPanel.GetComponentInChildren<Text>().text = pickUpable.number.ToString();
        numberPanel.GetComponentInChildren<Text>().enabled = true;

    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        numberPanel.GetComponent<Image>().enabled = false;
        numberPanel.GetComponentInChildren<Text>().enabled = false;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.RemoveItem(item);
    }

    public void UseItem()
    {
        if(item != null)
        {
            item.Use();
        }
    }
}
