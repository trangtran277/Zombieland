using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] float speed = 0.2f;
    [SerializeField] public float health = 100f;
    [SerializeField] float damageToPlayer = 10f;
    [SerializeField] float detectionDistance = 10f;
    [SerializeField] float fieldOfVision = 120f;
    [SerializeField] float attachRange = 0.9f;
    [SerializeField] float chaseTime = 10f;

    
    private Animator anim;
    private bool isAlive = true;
    private bool playerDetected = false;
    private NavMeshAgent agent;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            isAlive = false;
        }

        if(isAlive)
        {       
            Vector3 direction = player.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);
            float distFromPlayer = Vector3.Distance(player.position, this.transform.position);

            FindPlayer(distFromPlayer, angle);

            if (playerDetected)
            {
                agent.destination = player.position;
                anim.SetBool("isWalking", true);

                //attack when in range
                if (distFromPlayer <= attachRange)
                {
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", true);                
                }
                else
                {
                    anim.SetBool("isAttacking", false);
                }
            }
            else
            {
                agent.velocity = Vector3.zero;
                anim.SetBool("isWalking", false);
            }
        }        
    }

    private void FindPlayer(float distFromPlayer, float angle)
    {
        if (!playerDetected && distFromPlayer < detectionDistance && angle < fieldOfVision)
        {
            playerDetected = true;
            Invoke("StopChasing", chaseTime);
        }
    }

    private void StopChasing()
    {
        playerDetected = false;
    }
}
