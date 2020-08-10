using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] float speed = 0.2f;
    [SerializeField] float damageToPlayer = 1f;
    [SerializeField] float detectionDistance = 10f;
    [SerializeField] float fieldOfVision = 120f;
    [SerializeField] float playerDetectTime = 5f;
    [SerializeField] float chaseTime = 10f;
    [SerializeField] float attachRange = 0.9f;
    public ThirdPersonCharacter thirdPersonCharacter;
    public bool isAlive = true;
    
    private Animator anim;
    private bool playerDetected = false;
    private NavMeshAgent agent;
    private float timeToNextAttack;
    private float chaseBeginTime;
    private bool waitedBeforeChase = false;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) { player = GameObject.FindGameObjectWithTag("Player").transform; }
        if (thirdPersonCharacter == null) { thirdPersonCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>(); }
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        timeToNextAttack = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive)
        {
            isAlive = false;
            anim.SetTrigger("isDead");
            Invoke("DestroyEnemy", 3f);
        }
        else
        {       
            Vector3 direction = player.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);
            float distFromPlayer = Vector3.Distance(player.position, this.transform.position);

            FindPlayer(distFromPlayer, angle);

            if (playerDetected && Time.time >= chaseBeginTime && distFromPlayer < detectionDistance && angle < fieldOfVision && thirdPersonCharacter.health > 0)
            {
                agent.destination = player.position;
                anim.SetBool("isWalking", true);

                //attack when in range
                if (distFromPlayer <= attachRange)
                {
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", true);
                    if(Time.time > timeToNextAttack)
                    {
                        timeToNextAttack = Time.time + 1.5f;
                        thirdPersonCharacter.health -= damageToPlayer;
                    }
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
            chaseBeginTime = Time.time + playerDetectTime;
            Invoke("StopChasing", chaseTime + playerDetectTime); //still chase chaseTime after chasing started
            Debug.Log("Dected player");
        }
        Debug.Log("Attempt but did not detect player");
    }

    private void StopChasing()
    {
        playerDetected = false;
        waitedBeforeChase = false;
    }

    private void InflictDamageOnPlayer()
    {
        Debug.Log("Minus health");
        thirdPersonCharacter.health -= damageToPlayer;
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
