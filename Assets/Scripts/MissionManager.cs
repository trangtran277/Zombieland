using CompassNavigatorPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    #region Singleton
    public static MissionManager instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
    #endregion
    public Text dialog;
    public GameObject note1;
    public GameObject eastGate;
    public CompassProPOI eastGatePOI;
    public CompassProPOI parkingLotStorePOI;
    public Collectibles keyCardF;
    public bool isTriggerEastGate = false;
    public bool isTriggerWestGate = false;
    public GameObject westGate;
    public Collectibles keyCardA;
    public Collectibles keyCardB;
    public Collectibles keyCardC;
    public CompassProPOI westGatePOI;
    public Text mainMisson;
    public Text subMission;
    public Image icon;
    public PlayableDirector directorGate1;
    //public PlayableDirector directorGate2;
    Inventory inventory;
    bool hasKeyA = false;
    bool hasKeyB = false;
    bool hasKeyC = false;
    public GameObject triggerDeadEnd1;
    public GameObject triggerDeadEnd2;
    public bool isTriggerDeadEnd = false;

    private void Start()
    {
        inventory = Inventory.instance;
        mainMisson.text = "Unknown";
        subMission.text = "Read the note";
        
    }

    private void Update()
    {
        //eastGatePOI = eastGate.GetComponentInChildren<CompassProPOI>();
        if (note1 == null && eastGatePOI != null)
        {
            mainMisson.text = "Get out of this place";
            subMission.text = "Check out the East Gate";
            if (eastGatePOI != null)
                eastGatePOI.enabled = true;
        }

        if (eastGatePOI != null && eastGatePOI.enabled)
        {
            if (eastGatePOI.isVisited)
            {
                Destroy(eastGatePOI.gameObject);
                
                if (parkingLotStorePOI != null)
                    parkingLotStorePOI.enabled = true;
                subMission.text = "Get to the parking lot store";
            }
        }

        if(parkingLotStorePOI != null && parkingLotStorePOI.enabled)
        {
            if (parkingLotStorePOI.isVisited)
            {
                Destroy(parkingLotStorePOI.gameObject);
                subMission.text = "Search the store for the key card";
            }
        }

        if(parkingLotStorePOI == null && inventory.items.Contains(keyCardF))
        {
            subMission.text = "Unlock the East Gate";
            icon.enabled = false;
        }

        /*if (!isTriggerEastGate)
        {
            isTriggerWestGate = false;
            westGate.GetComponent<ThingsToInteract>().enabled = false;
            westGate.GetComponent<ToggleOutline>().enabled = false;
            westGate.GetComponent<cakeslice.Outline>().enabled = false;
        }*/
        if (isTriggerEastGate)
        {
            if(dialog.text.Equals("You: Hey! That's maybe the East Gate"))
            {
                dialog.transform.parent.gameObject.SetActive(true);
                dialog.text = "You: Damn! A dead end. There maybe another gate somewhere";
                StartCoroutine(DisableTip());

                subMission.text = "Find for another gate";

                westGate.GetComponent<ThingsToInteract>().enabled = true;
                westGate.GetComponent<ToggleOutline>().enabled = true;
                westGate.GetComponent<cakeslice.Outline>().enabled = true;
                triggerDeadEnd1.SetActive(true);
                triggerDeadEnd2.SetActive(true);

            }
            
            
        }
        if (isTriggerDeadEnd)
        {
            if (dialog.text.Equals("You: Damn! A dead end. There maybe another gate somewhere"))
            {
                dialog.transform.parent.gameObject.SetActive(true);
                dialog.text = "You: Must find another way around";
                StartCoroutine(DisableTip());
            }
            if (triggerDeadEnd1 != null)
                Destroy(triggerDeadEnd1);
            if (triggerDeadEnd2 != null)
                Destroy(triggerDeadEnd2);
        }
        if (inventory.items.Contains(keyCardA))
        {
            hasKeyA = true;
        }
        if (inventory.items.Contains(keyCardB))
        {
            hasKeyB = true;
        }
        if (inventory.items.Contains(keyCardC))
        {
            hasKeyC = true;
        }
        if (isTriggerEastGate && isTriggerWestGate)
        {
            if (triggerDeadEnd1 != null)
                Destroy(triggerDeadEnd1);
            if (triggerDeadEnd2 != null)
                Destroy(triggerDeadEnd2);
            if (hasKeyA && hasKeyB && hasKeyC)
            {
                icon.enabled = true;
                subMission.text = "Unlock the West Gate";
                westGatePOI.enabled = true;
                hasKeyA = false;
}
            else
            {
                if (dialog.text.Equals("You: Damn! A dead end. There maybe another gate somewhere") || dialog.text.Equals("You: Must find another way around"))
                {
                    dialog.transform.parent.gameObject.SetActive(true);
                    dialog.text = "You: Gotta find the key cards";
                    StartCoroutine(DisableTip());
                    subMission.text = "Search the whole area for the key cards";
                    icon.enabled = false;
                }    
            }  
        }
        
    }

    public void OnCompleted(ThingsToInteract thingsToInteract)
    {
        thingsToInteract.completed = false;
        thingsToInteract.enabled = false;
        thingsToInteract.GetComponent<ToggleOutline>().enabled = false;
        thingsToInteract.GetComponent<cakeslice.Outline>().enabled = false;
        thingsToInteract.GetComponent<BoxCollider>().enabled = false;
        if (thingsToInteract.name.Equals("East Gate"))
        {
            StartCoroutine(DelayCutscene());
        }
        else
        {
            StartCoroutine(DelayCutscene2());
            mainMisson.text = "Completed";
            subMission.enabled = false;
            icon.enabled = false;
            westGatePOI.enabled = false;

        }
    }
    IEnumerator DelayCutscene2()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Cutscene2Scene");
    }
    IEnumerator DelayCutscene()
    {
        yield return new WaitForSeconds(0.5f);
        directorGate1.Play();
    }
    IEnumerator DisableTip()
    {
        yield return new WaitForSeconds(5f);
        dialog.transform.parent.gameObject.SetActive(false);
    }
    IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(1.5f);
        //Debug.Log("exit");
        Application.Quit();
    }
}
