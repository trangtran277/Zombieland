using HutongGames.PlayMaker.Actions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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

    public GameObject weapon;

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
        if(weapon.activeInHierarchy)
        {
            Debug.Log("ok");
            if (ator.GetBool("crouch")) 
            {
                ator.SetTrigger("sitAttack");
                weapon.GetComponent<BoxCollider>().enabled = true;
            }else
            {
                ator.SetTrigger("standAttack");
                weapon.GetComponent<BoxCollider>().enabled = true;
            }
            
        }
        
     }
    IEnumerable WaitOneHitWeapon()
    {
        yield return new WaitForSeconds(1.367f);
        ator.SetBool("sitAttack", false);
    }
    public void StealClick()
    {
        //userControl.crouchswt = !userControl.crouchswt;
        Debug.Log("click steak");
        ator.SetBool("crouch", !ator.GetBool("crouch"));
    }
}
