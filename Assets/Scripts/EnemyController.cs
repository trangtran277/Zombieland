using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

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
    public ThirdPersonCharacter thirdPersonCharacter;
    
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
        thirdPersonCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            isAlive = false;
            anim.SetTrigger("isDead");
            Invoke("DestroyEnemy", 3f);
        }

        if(isAlive)
        {       
            Vector3 direction = player.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);
            float distFromPlayer = Vector3.Distance(player.position, this.transform.position);

            FindPlayer(distFromPlayer, angle);

            if (playerDetected && thirdPersonCharacter.health > 0)
            {
                agent.destination = player.position;
                anim.SetBool("isWalking", true);

                //attack when in range
                if (distFromPlayer <= attachRange)
                {
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", true);
                    Invoke("InflictDamageOnPlayer", 1.5f);
                }
                else
                {
                    anim.SetBool("isAttacking", false);
                }
            }
            else
            {
                agent.velocity = Vector3.zero;
                anim.SetBool("isAttacking", false);
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

    private void InflictDamageOnPlayer()
    {
        thirdPersonCharacter.health -= damageToPlayer;
    }

    private void DestroyEnemy()
    {
        Destroy(this);
    }
}
