using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class ConsumableItem : ScriptableObject
{
    new public string name = "New Consumable";
    public Sprite icon = null;
    public int maxNumber = 0;

    public virtual void TakeEffect()
    {
        Debug.Log("Effect taken");
        ThirdPersonCharacter player = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>();
        player.health += 10;
        if (player.health > player.maxhealth) player.health = player.maxhealth;
    }
}
