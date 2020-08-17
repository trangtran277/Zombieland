using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindZombiesInRadius : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        if(!FindZombies(player.transform.position,5f))
        {
            if (DetectionManager.instance.isNearDetected)
            {
                DetectionManager.instance.isNearDetected = false;
                DetectionManager.instance.SetDitection(false);
            }
            if(DetectionManager.instance.isBeingChased)
            {
                DetectionManager.instance.isBeingChased = false;
                DetectionManager.instance.SetChase(false);
            }
        }else
        {
            if(!DetectionManager.instance.isBeingChased)
            {
                DetectionManager.instance.isNearDetected = true;
                DetectionManager.instance.SetDitection(true);
            }
        }
    }
    bool FindZombies(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            //hitCollider.SendMessage("AddDamage");
            Debug.Log(hitCollider.gameObject.name);
            if(hitCollider.gameObject.CompareTag("Enemy"))
            {
                return true;
            }
        }
        return false;
    }
}
