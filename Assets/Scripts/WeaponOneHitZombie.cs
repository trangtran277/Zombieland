﻿using HutongGames.PlayMaker.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOneHitZombie : MonoBehaviour
{
    // Start is called before the first frame update
    //Animator animator;
    void Start()
    {
        //GameObject getPlayer = GameObject.FindGameObjectWithTag("Player");
        //animator = getPlayer.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("sitAttck"));
        // neu ma dg danh va riu trung thi moi chet zombie
        

        if (other.CompareTag("Enemy"))
        {
            if (GetComponent<BoxCollider>().enabled)
            {
                //Destroy(other.gameObject);
                //other.gameObject.GetComponent<EnemyControlPatrolPath>().isAlive = false;
                other.gameObject.GetComponent<EnemyAI>().isAlive = false;
                Destroy(EquipmentManager.instance.currentEquipment[3]);
                InventoryUI.instance.UpdateEquipmentSlot(null, null);
                GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>()[6].Play();
                //GameObject.FindGameObjectsWithTag("Weapon")[0].SetActive(false);
                
            }
            else
                GetComponent<BoxCollider>().enabled = false;
        }else
            GetComponent<BoxCollider>().enabled = false;

        GetComponent<BoxCollider>().enabled = false;
    }

}
