using HutongGames.PlayMaker.Actions;
using System.Collections;
using System.Collections.Generic;
//using System.Runtime.Remoting.Metadata.W3cXsd2001;
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

    public Button bag, attack, run, pick, steal;
    public InventoryUI inventoryUI;
    public ThirdPersonUserControl userControl;
    private Animator ator;
    private EquipmentManager equipmentManager;
    public ThingsToInteract curInteract;
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

    private void Update()
    {
        if(equipmentManager.currentEquipment[3] == null)
        {
            attack.interactable = false;
        }
        else
        {
            attack.interactable = true;
        }
    }
    public void RunClick()
    {

    }
    public void BagClick()
    {
        //Debug.Log("bag click");
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
        Debug.Log("Attacked");
        //play attack animation here
        if (weapon.activeInHierarchy)
        {
            //Debug.Log("ok");
            if (ator.GetBool("crouch"))
            {
                ator.SetTrigger("sitAttack");
                weapon.GetComponent<BoxCollider>().enabled = true;
            }
            else
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
        //Debug.Log("click steak");
        if (!ator.GetBool("crouch"))
        {
            userControl.GetComponent<CapsuleCollider>().height = 2.5f;
            userControl.GetComponent<CapsuleCollider>().center = new Vector3(-0.01751587f, 1f, 0.02719201f);
        }
        else
        {
            userControl.GetComponent<CapsuleCollider>().height = 5f;
            userControl.GetComponent<CapsuleCollider>().center = new Vector3(-0.01751587f, 2.4f, 0.02719201f);
        }
        
        ator.SetBool("crouch", !ator.GetBool("crouch"));
    }
}
