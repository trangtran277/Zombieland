using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Inventory/Resource")]
public class ResourceItem : ScriptableObject
{
    new public string name = "New Resource";
    public Sprite icon = null;
    public int maxNumber = 0;
    public string description="";
}
