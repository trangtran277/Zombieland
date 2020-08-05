using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManagerController : MonoBehaviour
{
     // Start is called before the first frame update
     public Button bag, attack, run, pick;
     public GameObject human;
     private InventoryUI inventoryUI;
     private Animator ator;
     void Start()
     {
          ator = human.GetComponent<Animator>();
     }

     // Update is called once per frame
     void Update()
     {

     }
     public void RunClick()
     {
          run.GetComponent<Image>().color = !ator.GetBool("run") ? Color.red : Color.white;
          ator.SetBool("run", !ator.GetBool("run"));
     }
     public void BagClick()
     {
          //inventoryUI.ToggleInventory();
     }
     public void PickClick()
     {

     }
     public void AttackClick()
     {

     }
}
