using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    public Equipment[] currentEquipment;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;
    void Start()
    {
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        //for (int i = 0; i< numSlots; i++)
        //{
            //currentEquipment[i] = null;
        //}
    }

    public void Equip(Equipment equipment)
    {
        int slotIndex = 3;
        Equipment oldItem = null;
        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            Inventory.instance.AddItem(oldItem);
        }
   
        currentEquipment[slotIndex] = equipment;
        Inventory.instance.RemoveItem(equipment);
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(equipment, oldItem);
        }
    }

    public void Unequip(int slotIndex)
    {
        if(currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            Inventory.instance.AddItem(oldItem);

            currentEquipment[slotIndex] = null;
            if(onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
        }
    }

    public void UnequipAll()
    {
        for(int i=0; i<currentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
            UnequipAll();
    }
}
