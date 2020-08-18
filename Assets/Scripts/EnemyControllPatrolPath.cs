using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityScript.Macros;

public class enemyController1 : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public float playerDistance;
    public float awareAI = 10f;
    public float AIMoveSpeed;
    public float damping = 6f;
    public float fieldOfVision = 120f;

    public Transform[] navPoints;
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform goal;

    Animator ani;


    void Start()
    {
        agent.destination = navPoints[Ramdom()].position;

        ani = this.GetComponent<Animator>();
        ani.SetBool("isWalk", true);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void GotoNextPoint()
    {
        
    }
    void Chase()
    {
        transform.Translate(Vector3.forward * AIMoveSpeed * Time.deltaTime);
    }
    int Ramdom()
    {
        return Random.Range(0, navPoints.Length);
    }
}
