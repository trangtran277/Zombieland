using HutongGames.PlayMaker.Actions;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] float speed = 0.2f;
    [SerializeField] float damageToPlayer = 1f;
    [SerializeField] float detectionDistance = 1f;
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
    private float stopTime = 0;
    private bool chasePlayer = false;
    private float timer = 0;
    private float healthofPlayer;
    public bool playerAlive = true;



    // Start is called before the first frame update
    void Start()
    {
        healthofPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>().health;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; 
        }
        if (thirdPersonCharacter == null)
        {
            thirdPersonCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>();
        }
        anim = this.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        timeToNextAttack = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(healthofPlayer);
        
        if (!isAlive)
        {
            isAlive = false;
            anim.SetTrigger("isDead");
            Invoke("DestroyEnemy", 3f);
        }
        else
        {
            
            // Debug.Log("go here");
            Vector3 direction = player.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);
            float distFromPlayer = Vector3.Distance(player.position,this.transform.position);

            Debug.Log(FindPlayer(this.transform));

            if (distFromPlayer <= detectionDistance && FindPlayer(this.transform))
            {
                agent.enabled = true;
                //canh bao vao tam cua zombie o day
                if(!DetectionManager.instance.isNearDetected)
                {
                    DetectionManager.instance.isNearDetected = true;
                    DetectionManager.instance.SetDitection(true);
                }
                
                if (angle <= fieldOfVision / 2)
                {
                    GetComponent<BoxCollider>().isTrigger = false;
                    //bat canh bao bi zombie duoi
                    if (!DetectionManager.instance.isBeingChased)
                    {
                        DetectionManager.instance.isBeingChased = true;
                        DetectionManager.instance.SetChase(true);
                        DetectionManager.instance.isNearDetected = false;
                        DetectionManager.instance.SetDitection(false);
                    }
                    //follow
                    anim.SetBool("isWalking", true);
                    agent.destination = player.position;
                    FaceTarget();
                    if(playerAlive)
                    healthofPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>().health;
                    if (distFromPlayer <= agent.stoppingDistance)
                    {
                        //attack
                        FaceTarget();
                        anim.SetBool("isWalking", false);

                        if (healthofPlayer > 0)
                            anim.SetBool("isAttacking", true);
                    }
                    else if(healthofPlayer>0)
                    {
                        anim.SetBool("isAttacking", false);
                        anim.SetBool("isWalking", true);
                    }
                }
                else
                {
                    GetComponent<BoxCollider>().isTrigger = true;
                    agent.enabled = false;
                    anim.SetBool("isWalking", false);
                    // ra khoi tam
                    // tat canh bao bi zombie duoi o day
                    if (DetectionManager.instance.isBeingChased)
                    {
                        DetectionManager.instance.isBeingChased = false;
                        DetectionManager.instance.SetChase(false);
                    }
                    
                }
            }else
            {
                GetComponent<BoxCollider>().isTrigger = true;
                anim.SetBool("isWalking", false);

            }



            /*if(playerDetected && !chasePlayer)
            {
                if(timer < playerDetectTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    chasePlayer = true;
                    timer = 0;
                    stopTime = Time.time + chaseTime;
                }
            }

            if(!playerDetected)
            {
                timer = 0;
            }

            if (chasePlayer && Time.time < stopTime && thirdPersonCharacter.health > 0)
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
                chasePlayer = false;
                agent.velocity = Vector3.zero;
                anim.SetBool("isAttacking", false);
                anim.SetBool("isWalking", false);
            }*/
        }
    }
    private void FixedUpdate()
    {
        if(GameObject.FindGameObjectWithTag("Player"))
        healthofPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>().health;
        if(healthofPlayer<=0)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);
            playerAlive = false;
        }
    }
    void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    private void FindPlayer(float distFromPlayer, float angle)
    {
        if (!playerDetected && distFromPlayer < detectionDistance && angle < fieldOfVision/2)
        {
            Debug.Log("Dected player");
            playerDetected = true;
            //Invoke("StopChasing", chaseTime + playerDetectTime); //still chase chaseTime after chasing started
            
        }
        else
        {
            playerDetected = false;
        }
    }

    /*private void StopChasing()
    {
        playerDetected = false;
    }

    private void InflictDamageOnPlayer()
    {
        Debug.Log("Minus health");
        thirdPersonCharacter.health -= damageToPlayer;
    }
    */
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    private bool FindPlayer(Transform target)
    {
        RaycastHit hit;
        for (float x=-1f;x<=1f;x+=0.2f)
        {
            if (Physics.Raycast(target.position, target.TransformDirection(new Vector3(x, 0f, 1f)), out hit, detectionDistance + 50))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
