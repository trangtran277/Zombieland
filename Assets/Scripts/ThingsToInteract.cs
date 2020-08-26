using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThingsToInteract : MonoBehaviour, IInteractable
{
    public new string name; 
    public List<Collectibles> input = new List<Collectibles>();
    public List<Collectibles> output = new List<Collectibles>();
    public List<Collectibles> collected = new List<Collectibles>();
    public string result;
    public Sprite icon;
    InventoryUI inventoryUI;
    InteractionDes interactionDes;
    public bool completed = false;

    void Start()
    {
        inventoryUI = InventoryUI.instance;
        interactionDes = inventoryUI.interactionDes.GetComponent<InteractionDes>();
    }
    public void Interact()
    {
        inventoryUI.ToggleInventory(1);
        UpdateInteractionUI();
    }

    public void UpdateInteractionUI()
    {
        //Debug.Log("Interaction update");
        interactionDes.itemIcon.sprite = icon;
        interactionDes.itemName.text = name;
        if (input.Count > 0)
        {
            if (input.Count == 1)
            {
                interactionDes.inputIcons[0].transform.parent.gameObject.SetActive(true);
                interactionDes.inputIcons[0].sprite = input[0].item.icon;
                interactionDes.inputIcons[1].transform.parent.gameObject.SetActive(false);
                interactionDes.inputIcons[2].transform.parent.gameObject.SetActive(false);
            }
            if (input.Count == 2)
            {
                interactionDes.inputIcons[0].transform.parent.gameObject.SetActive(true);
                interactionDes.inputIcons[0].sprite = input[0].item.icon;
                interactionDes.inputIcons[1].transform.parent.gameObject.SetActive(true);
                interactionDes.inputIcons[1].sprite = input[1].item.icon;
                interactionDes.inputIcons[2].transform.parent.gameObject.SetActive(false);
            }
            if (input.Count == 3)
            {
                interactionDes.inputIcons[0].transform.parent.gameObject.SetActive(true);
                interactionDes.inputIcons[0].sprite = input[0].item.icon;
                interactionDes.inputIcons[1].transform.parent.gameObject.SetActive(true);
                interactionDes.inputIcons[1].sprite = input[1].item.icon;
                interactionDes.inputIcons[2].transform.parent.gameObject.SetActive(true);
                interactionDes.inputIcons[2].sprite = input[2].item.icon;
            }
        }
        else
        {
            interactionDes.inputIcons[0].transform.parent.gameObject.SetActive(false);
            interactionDes.inputIcons[1].transform.parent.gameObject.SetActive(false);
            interactionDes.inputIcons[2].transform.parent.gameObject.SetActive(false);
        }

        for (int i = 0; i < input.Count; i++)
        {
            if (collected.Contains(input[i]))
            {
                interactionDes.inputIcons[i].color = new Color32(126, 118, 118, 255);
            }
            else
            {
                interactionDes.inputIcons[i].color = new Color32(255, 255, 255, 255);
            }
        }

        if (output.Count > 0)
        {
            interactionDes.resText.gameObject.SetActive(false);
            interactionDes.toGetText.gameObject.SetActive(true);
            interactionDes.outputIcon.gameObject.transform.parent.gameObject.SetActive(true);
            interactionDes.outputIcon.sprite = output[0].item.icon;
            if (completed)
            {
                interactionDes.outputIcon.color = new Color32(126, 118, 118, 255);
            }
        }
        else
        {
            interactionDes.resText.gameObject.SetActive(true);
            interactionDes.resText.text = result;
            interactionDes.toGetText.gameObject.SetActive(false);
            interactionDes.outputIcon.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    public void OnCompleted()
    {
        inventoryUI.ToggleInventory();
        if (output.Count > 0)
        {
            Inventory.instance.AddItem(output[0]);
        }
        else
        {
            MissionManager.instance.OnCompleted(this);
        }
    } 
}
