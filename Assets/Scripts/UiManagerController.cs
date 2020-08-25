using HutongGames.PlayMaker.Actions;
using System.Collections;
using System.Collections.Generic;
//using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.Audio;

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

    public Button bag, attack, run, crouch, collect;
    public Image attackButtonIcon;
    public Sprite attackSprite, collectSprite, sleepSprite, readSprite;
    public InventoryUI inventoryUI;
    public ThirdPersonUserControl userControl;
    private EquipmentManager equipmentManager;
    public ThingsToInteract curInteract;
    public GameObject weapon;
    public Animator ator;
    private GameManager manager;
    private AudioSource[] audios;

    public float curDistance;
    public float detectionDistanceModifier;
    public float fieldOfVisionModifier;
    public float rayCastPointModifier;

    float nextAttackTime = 0;
    public float eachAttackTime = 2f;
    void Start()
    {
        audios = GetComponents<AudioSource>();
        manager = GameManager.instance;
        equipmentManager = EquipmentManager.instance;
        bag.onClick.AddListener(BagClick);
        attack.onClick.AddListener(AttackClick);
        crouch.onClick.AddListener(CrouchClick);
        collect.onClick.AddListener(AttackClick);

        detectionDistanceModifier = 1f;
        fieldOfVisionModifier = 1f;
        rayCastPointModifier = 1.5f;
    }

    private void Update()
    {
        IInteractable itemFound = userControl.CheckItemAround();
        if (itemFound != null)
        {
            if (itemFound is ThingsToInteract)
            {
                attackButtonIcon.sprite = collectSprite;
            }
            if(itemFound is Collectibles)
            {
                attackButtonIcon.sprite = collectSprite ;
            }
            if(itemFound is Bed)
            {
                attackButtonIcon.sprite = sleepSprite;
            }
            if (itemFound is NoteObject)
            {
                attackButtonIcon.sprite = readSprite;
            }
        }
        else
        {
            attackButtonIcon.sprite = attackSprite;
        }

        if (equipmentManager.currentEquipment[3] == null && attackButtonIcon.sprite == attackSprite)
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
        if (manager.gameEnded)
            return;
        inventoryUI.ToggleInventory();
        curInteract = null;
        audios[1].Play();
    }
    /*public void InteractClick()
    {
        IInteractable itemFound = userControl.CheckItemAround();
        if (itemFound != null)
        {
            if (itemFound is ThingsToInteract)
                curInteract = itemFound as ThingsToInteract;
            itemFound.Interact();
        }
        //collect.gameObject.SetActive(false);
    }*/
     public void AttackClick()
     {
        if (manager.gameEnded)
            return;
        IInteractable itemFound = userControl.CheckItemAround();
        if (itemFound != null)
        {
            if (itemFound is ThingsToInteract)
                curInteract = itemFound as ThingsToInteract;
            else if (itemFound is Collectibles)
                audios[0].Play();
            else if (itemFound is NoteObject)
                audios[2].Play();
            else
                audios[3].Play();
            itemFound.Interact();
            
        }
        else 
        {
            if (Time.time > nextAttackTime)
            {
                if (weapon.activeSelf)
                {
                    if (ator.GetBool("crouch"))
                    {
                        ator.SetTrigger("sitAttack");
                    }
                    else
                    {
                        ator.SetTrigger("standAttack");
                    }
                    //weapon.GetComponent<BoxCollider>().enabled = true;
                    StartCoroutine(EnableBoxCollider());
                    nextAttackTime = Time.time + eachAttackTime;
                }
            }
        }

        
    }
    IEnumerator EnableBoxCollider()
    {
        yield return new WaitForSeconds(0.5f);
        weapon.GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(1.5f);
        weapon.GetComponent<BoxCollider>().enabled = false;
    }
    
    public void CrouchClick()
    {
        if (manager.gameEnded)
            return;
        
        if (!ator.GetBool("crouch"))
        {
            userControl.GetComponent<CapsuleCollider>().height = 2.7f;
            userControl.GetComponent<CapsuleCollider>().center = new Vector3(-0.01751587f, 1.3f, 0.02719201f);
            detectionDistanceModifier = 0.5f;
            fieldOfVisionModifier = 0.7f;
            rayCastPointModifier = 0.7f;
            
        }
        else
        {
            userControl.GetComponent<CapsuleCollider>().height = 4.8f;
            userControl.GetComponent<CapsuleCollider>().center = new Vector3(-0.01751587f, 2.4f, 0.02719201f);
            detectionDistanceModifier = 1f;
            fieldOfVisionModifier = 0.7f;
            rayCastPointModifier = 1.4f;
        }
        
        ator.SetBool("crouch", !ator.GetBool("crouch"));
    }
}
