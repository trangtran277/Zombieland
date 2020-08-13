using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class RightHandZombieAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("1hit rightthand");
            other.gameObject.GetComponent<ThirdPersonCharacter>().health -= 10;
        }
    }
    
}
