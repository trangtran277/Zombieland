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

    public Button bag, attack, run, crouch, collect;
    public Image attackButtonIcon;
    public Sprite attackSprite, collectSprite, sleepSprite, readSprite;
    public InventoryUI inventoryUI;
    public ThirdPersonUserControl userControl;
    private EquipmentManager equipmentManager;
    public ThingsToInteract curInteract;
    public GameObject weapon;
    public Animator ator;

    //GameObject[] enemys; //= new GameObject[1000];
    //Animator ani;
    public float curDistance;
    public float detectionDistanceModifier;
    public float fieldOfVisionModifier;
    public float rayCastPointModifier;

    float nextAttackTime = 0;
    public float eachAttackTime = 2f;
    void Start()
    {
        //ator = human.GetComponent<Animator>();
        equipmentManager = EquipmentManager.instance;
        bag.onClick.AddListener(BagClick);
        attack.onClick.AddListener(AttackClick);
        crouch.onClick.AddListener(CrouchClick);
        collect.onClick.AddListener(AttackClick);

        detectionDistanceModifier = 1f;
        fieldOfVisionModifier = 1f;
        rayCastPointModifier = 1.5f;
        //ani = human.GetComponent<Animator>();
        /*enemys = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemys[0].GetComponent<EnemyControlPatrolPath>() != null)
        {
            distancePlayertoZombieInit = enemys[0].GetComponent<EnemyControlPatrolPath>().detectionDistance;
            fieldOfVisionInit = enemys[0].GetComponent<EnemyControlPatrolPath>().fieldOfVision;
        }
        else
        {
            distancePlayertoZombieInit = enemys[0].GetComponent<EnemyController>().detectionDistance;
            fieldOfVisionInit = enemys[0].GetComponent<EnemyController>().fieldOfVision;
        }*/
        //curDistance = distancePlayertoZombieInit;
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
        inventoryUI.ToggleInventory();
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
        IInteractable itemFound = userControl.CheckItemAround();
        if (itemFound != null)
        {
            if (itemFound is ThingsToInteract)
                curInteract = itemFound as ThingsToInteract;
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
                    weapon.GetComponent<BoxCollider>().enabled = true;
                    StartCoroutine(EnableBoxCollider());
                    nextAttackTime = Time.time + eachAttackTime;
                }
            }
        }

        
    }
    IEnumerator EnableBoxCollider()
    {
        //yield return new WaitForSeconds(1f);
        //weapon.GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(1.5f);
        weapon.GetComponent<BoxCollider>().enabled = false;
    }
    /*IEnumerable WaitOneHitWeapon()
    {
        yield return new WaitForSeconds(1.367f);
        ator.SetBool("sitAttack", false);
    }*/
    public void CrouchClick()
    {
        /*enemys = GameObject.FindGameObjectsWithTag("Enemy");
        if (!ani.GetBool("crouch"))
        {
            foreach (GameObject e in enemys)
            {
                if (e.GetComponent<EnemyControlPatrolPath>() != null)
                {
                    e.GetComponent<EnemyControlPatrolPath>().fieldOfVision = fieldOfVisionInit / 2;
                    e.GetComponent<EnemyControlPatrolPath>().detectionDistance = distancePlayertoZombieInit / 2;
                }
                else
                {
                    e.GetComponent<EnemyController>().fieldOfVision = fieldOfVisionInit / 2;
                    e.GetComponent<EnemyController>().detectionDistance = distancePlayertoZombieInit / 2;
                }
            }
            curDistance = distancePlayertoZombieInit / 2;
        }
        else
        {
            foreach (GameObject e in enemys)
            {
                if (e.GetComponent<EnemyControlPatrolPath>() != null)
                {
                    e.GetComponent<EnemyControlPatrolPath>().fieldOfVision = fieldOfVisionInit;
                    e.GetComponent<EnemyControlPatrolPath>().detectionDistance = distancePlayertoZombieInit;
                }
                else
                {
                    e.GetComponent<EnemyController>().fieldOfVision = fieldOfVisionInit;
                    e.GetComponent<EnemyController>().detectionDistance = distancePlayertoZombieInit;
                }
            }
            curDistance = distancePlayertoZombieInit;
        }*/
        //userControl.crouchswt = !userControl.crouchswt;
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
