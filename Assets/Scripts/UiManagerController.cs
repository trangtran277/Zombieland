using HutongGames.PlayMaker.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class UiManagerController : MonoBehaviour
{
     // Start is called before the first frame update
     public Button bag, attack, run, pick,steal;
     public InventoryUI inventoryUI;
     public ThirdPersonUserControl userControl;
     private Animator ator;
     private EquipmentManager equipmentManager;
    public GameObject human;
     void Start()
     {
        ator = human.GetComponent<Animator>();
        equipmentManager = EquipmentManager.instance;
        bag.onClick.AddListener(BagClick);
        attack.onClick.AddListener(AttackClick);
        pick.onClick.AddListener(PickClick);
        steal.onClick.AddListener(StealClick);
     }

     // Update is called once per frame
     void Update()
     {

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
        Debug.Log("click pick");
        Interactable itemFound = userControl.CheckItemAround();
        if (itemFound != null)
        {
                //userControl.interactionCircle.SetActive(false);
                itemFound.Interact();
        }
    }
     public void AttackClick()
     {
        Debug.Log("click attack");
        ator.SetTrigger("attack");
        //Debug.Log("Attacked");
        //play attack animation here
        float damage = userControl.m_Character.baseDamge;
        Equipment weapon = EquipmentManager.instance.currentEquipment[(int)EquipmentSlot.Weapon];
        if (weapon != null)
        {
            damage += weapon.equipmentItem.damage;
        }
        EnemyController enemy = userControl.CheckEnemyAround();
        if(enemy != null)
        {
            enemy.health -= damage;
        }
        
     }
    public void StealClick()
    {
        userControl.crouchswt = !userControl.crouchswt;
        Debug.Log("click steak");
    }
}
