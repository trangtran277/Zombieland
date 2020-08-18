using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityScript.Macros;

public class EnemeControlPatrolPath : MonoBehaviour
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
        agent.speed = AIMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance<=0.5f)
        {
            GotoNextPoint();
        }
    }
    void GotoNextPoint()
    {
        agent.destination = navPoints[Ramdom()].position;
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
