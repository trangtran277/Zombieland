using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindZombiesInRadius : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject player;
    public float distanceToaZombie =10f;

    
    private void Start()
    {
        player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (FindZombies(player.transform.position,distanceToaZombie).Count<=0)
        {
            if(DetectionManager.instance.isNearDetected)
            {
                DetectionManager.instance.isNearDetected = false;
                DetectionManager.instance.SetDitection(false);
            }
            if(DetectionManager.instance.isBeingChased)
            {
                DetectionManager.instance.isBeingChased = false;
                DetectionManager.instance.SetChase(false);
            }
        }
        else
        {
            foreach (GameObject g in FindZombies(player.transform.position, distanceToaZombie))
            {
                //Instantiate(indicatorPrefab, holder.transform);
                Vector3 screenPoint = Camera.main.WorldToViewportPoint(g.transform.position);
                if (!(screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1))
                {
                    IndicatorSys.CreateIndicator(g.transform, this.transform);
                }
            }
            if (!DetectionManager.instance.isBeingChased)
            {
                DetectionManager.instance.isNearDetected = true;
                DetectionManager.instance.SetDitection(true);
                
            }
        }
    }
    List<GameObject> FindZombies(Vector3 center, float radius)
    {
        List<GameObject> enemy_ar = new List<GameObject>();
        enemy_ar.Clear();
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            //hitCollider.SendMessage("AddDamage");
            //Debug.Log(hitCollider.gameObject.name);
            if(hitCollider.gameObject.CompareTag("Enemy"))
            {
                enemy_ar.Add(hitCollider.gameObject);
                //return true;
            }
        }
        //return false;
        return enemy_ar;
    }
}
