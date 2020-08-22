using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class RightHandZombieAttack : MonoBehaviour
{
    private void Start()
    {
        enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("1hit rightthand");
            other.gameObject.GetComponent<ThirdPersonCharacter>().health -= 10;
        }
    }
    
}
