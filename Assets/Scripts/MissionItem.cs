using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionItem : Collectibles
{
    public override void Use()
    {
        ThingsToInteract interact = UiManagerController.instance.curInteract;
        if(interact != null)
        {
            if(interact.input.Contains(this))
            {
                interact.collected.Add(this);
                RemoveFromInventory();
                interact.UpdateInteractionUI();
                if (interact.collected.Count == interact.input.Count)
                {
                    interact.completed = true;
                    interact.OnCompleted();
                }
                
            }
            else
            {
                Debug.Log("Cannot use on this");
            }
            
        }
    }
    /*List<PickUpable> enough = new List<PickUpable>();
    bool isEnough = true;
        foreach(var inItem in input)
        {
            PickUpable itemFound = Inventory.instance.items.Find(i => i.GetName().Equals(inItem.GetName()));
            if(itemFound != null)
            {
                if(itemFound.number >= inItem.number)
                {
                    enough.Add(itemFound);
                    continue;              
                }
                else
                {
                    isEnough = false;
                    break;
                }
            }
            else
            {
                isEnough = false;
                break;
            }
        }

        if (isEnough)
        {
            for(int i=0; i<enough.Count; i++)
            {
                enough[i].number -= input[i].number;
                if(enough[i].number == 0)
                {
                    enough[i].RemoveFromInventory();
                }
                else
                {
                    Inventory.instance.ItemChanged(enough[i], enough[i].number);
                }
            }

            foreach(var outItem in output)
            {
                Inventory.instance.AddItem(outItem);
            }
        }
        else
        {
            Debug.Log("Not enough resources");
        }*/
}
