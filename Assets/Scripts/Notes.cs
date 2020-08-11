using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Inventory/Note")]
public class Notes : ScriptableObject
{
    public string title;
    [TextArea]
    public string content;
    
}
