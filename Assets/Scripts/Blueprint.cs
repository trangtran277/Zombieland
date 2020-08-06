using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Blueprint", menuName = "Inventory/Blueprint")]
public class Blueprint : ScriptableObject
{
    new public string name = "New Blueprint";
    public Sprite icon = null;
    public int maxNumber = 1;
    public string description = "";
}
