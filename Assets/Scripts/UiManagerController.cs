using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class UiManagerController : MonoBehaviour
{
    #region Singleton
    public static UiManagerController instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
    #endregion

    public Button bag, attack, run, pick;
     public InventoryUI inventoryUI;
     public ThirdPersonUserControl userControl;
     private Animator ator;
     private EquipmentManager equipmentManager;
    public ThingsToInteract curInteract;
    void Start()
     {
        equipmentManager = EquipmentManager.instance;
        bag.onClick.AddListener(BagClick);
        attack.onClick.AddListener(AttackClick);
        pick.onClick.AddListener(PickClick);
     }
     public void RunClick()
     {

     }
     public void BagClick()
     {
        Debug.Log("bag click");
        inventoryUI.ToggleInventory();
     }
     public void PickClick()
     {
        IInteractable itemFound = userControl.CheckItemAround();
        if (itemFound != null)
        {
            //userControl.interactionCircle.SetActive(false);
            if (itemFound is ThingsToInteract)
                curInteract = itemFound as ThingsToInteract;
            itemFound.Interact();
        }
    }
     public void AttackClick()
     {
        //Debug.Log("Attacked");
        //play attack animation here
        /*float damage = userControl.m_Character.baseDamge;
        Equipment weapon = EquipmentManager.instance.currentEquipment[(int)EquipmentSlot.Weapon];
        if (weapon != null)
        {
            damage += weapon.equipmentItem.damage;
        }
        EnemyController enemy = userControl.CheckEnemyAround();
        if(enemy != null)
        {
            enemy.isAlive = false;
        }
        }*/        
     }
}
