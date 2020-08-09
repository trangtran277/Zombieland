using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBlueprint : PickUpable
{
    /*public Blueprint blueprint;
    public List<PickUpable> input = new List<PickUpable>();
    public List<PickUpable> output = new List<PickUpable>();
    public override void Use()
    {
        base.Use();
        List<PickUpable> enough = new List<PickUpable>();
        bool isEnough = true;
        foreach (var inItem in input)
        {
            PickUpable itemFound = Inventory.instance.items.Find(i => i.GetName().Equals(inItem.GetName()));
            if (itemFound != null)
            {
                if (itemFound.number >= inItem.number)
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
            for (int i = 0; i < enough.Count; i++)
            {
                enough[i].number -= input[i].number;
                if (enough[i].number == 0)
                {
                    enough[i].RemoveFromInventory();
                }
                else
                {
                    Inventory.instance.ItemChanged(enough[i], enough[i].number);
                }
            }

            foreach (var outItem in output)
            {
                Inventory.instance.AddItem(outItem);
            }
        }
        else
        {
            Debug.Log("Not enough resources");
        }
    }

    public override string GetName()
    {
        return base.GetName() + blueprint.name;
    }

    public override int GetMaxNumber()
    {
        return base.GetMaxNumber() + blueprint.maxNumber;
    }

    public override Sprite GetSprite()
    {
        return blueprint.icon;
    }*/
}
