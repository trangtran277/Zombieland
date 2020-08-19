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

    public Button bag, attack, run, crouch, collect; //interact
    public Sprite attackSprite, collectSprite, sleepSprite, readSprite;
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
        //interact.onClick.AddListener(InteractClick);
        crouch.onClick.AddListener(CrouchClick);
        collect.onClick.AddListener(AttackClick);
    }

    private void Update()
    {
        IInteractable itemFound = userControl.CheckItemAround();
        if (itemFound != null)
        {
            if (itemFound is ThingsToInteract)
            {
                //curInteract = itemFound as ThingsToInteract;
                attack.GetComponentsInChildren<Image>()[1].sprite = collectSprite;
            }
            if(itemFound is Collectibles)
            {
                attack.GetComponentsInChildren<Image>()[1].sprite = collectSprite ;
            }
            if(itemFound is Bed)
            {
                attack.GetComponentsInChildren<Image>()[1].sprite = sleepSprite;
            }
            if (itemFound is NoteObject)
            {
                attack.GetComponentsInChildren<Image>()[1].sprite = readSprite;
            }
        }
        else
        {
            attack.GetComponentsInChildren<Image>()[1].sprite = attackSprite;
        }

        if (equipmentManager.currentEquipment[3] == null && attack.GetComponentsInChildren<Image>()[1].sprite == attackSprite)
        {
            attack.interactable = false;
        }
        else
        {
            attack.interactable = true;
        }
    }
  
    public void BagClick()
    {
        inventoryUI.ToggleInventory();
    }
    public void InteractClick()
    {
        IInteractable itemFound = userControl.CheckItemAround();
        if (itemFound != null)
        {
            //userControl.interactionCircle.SetActive(false);
            if (itemFound is ThingsToInteract)
                curInteract = itemFound as ThingsToInteract;
            itemFound.Interact();
        }
        collect.gameObject.SetActive(false);
    }
     public void AttackClick()
     {
        IInteractable itemFound = userControl.CheckItemAround();
        if (itemFound != null)
        {
            if (itemFound is ThingsToInteract)
                curInteract = itemFound as ThingsToInteract;
            itemFound.Interact();
            
        }
        else 
        {
            if (weapon.activeInHierarchy)
            {
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

        
    }
    IEnumerable WaitOneHitWeapon()
    {
        yield return new WaitForSeconds(1.367f);
        ator.SetBool("sitAttack", false);
    }
    public void CrouchClick()
    {
        //userControl.crouchswt = !userControl.crouchswt;
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
