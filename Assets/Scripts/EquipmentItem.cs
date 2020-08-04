using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class EquipmentItem : ScriptableObject
{
    new public string name = "New Equipment";
    public Sprite icon = null;
    public int armor = 0;
    public int damage = 0;
    public EquipmentSlot equipmentSlot;
}

public enum EquipmentSlot { Head, Chest, Legs, Weapon, Feet }