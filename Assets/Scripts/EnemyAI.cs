using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Patrol,
        Chase,
        Attack,
        GoToLastSighting
    }
    public Transform player;
    public float detectionDistance = 10f;
    public float fieldOfVision = 120f;
    public float stopChasingDistance = 17f;
    public float attackRange = 1f;
    public float eachAttackTime = 2f;
    public float damage = 20f;
    public float speedWhileChase;
    public float speedWhilePatrol;
    public bool isAlive = true;
    public Transform[] PatrolPoints;
    public int curIndex = -1;

    DetectionManager detectionManager;
    UiManagerController uiManager;
    ThirdPersonCharacter character;

    private Animator anim;
    private NavMeshAgent agent;
    private State curState;
    private float nextAttackTime = 0;
    private Vector3 lastSighting;

    void Start()
    {
        detectionManager = DetectionManager.instance;
        uiManager = UiManagerController.instance;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>();
        
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speedWhilePatrol;
        if (PatrolPoints.Length > 0)
        {
            curIndex = 0;
        }
        curState = State.Patrol;
        nextAttackTime = Time.time;
    }
    void Update()
    {
        if (!isAlive)
        {
            agent.isStopped = true;
            anim.SetTrigger("isDead");
            Invoke("DestroyEnemy", 3f);
        }
        else
        {
            switch (curState)
            {
                default:
                case State.Patrol:
                    if (curIndex >= 0)
                    {
                        anim.SetBool("isWalk", true);
                        agent.isStopped = false;
                        agent.destination = PatrolPoints[curIndex].position;
                        agent.speed = speedWhilePatrol;
                        if (agent.remainingDistance <= 1f)
                        {
                            curIndex += 1;
                            if (curIndex > PatrolPoints.Length - 1)
                                curIndex = 0;
                            agent.destination = PatrolPoints[curIndex].position;
                        }
                    }
                    else
                    {
                        agent.isStopped = true;
                    }
                    //FindTarget();
                    break;
                case State.Chase:
                    if (character.health > 0)
                    {
                        anim.SetBool("isWalk", false);
                        anim.SetBool("isWalking", true);
                        agent.isStopped = false;
                        agent.destination = player.position;
                        agent.speed = speedWhileChase;
                        if (agent.remainingDistance <= attackRange)
                        {
                            anim.SetBool("isWalking", false);
                            agent.isStopped = true;
                            curState = State.Attack;
                        }
                        if (!FindPlayer(stopChasingDistance))
                        {
                            curState = State.GoToLastSighting;
                        }
                    }
                    else
                    {
                        anim.SetBool("isWalking", false);
                        agent.isStopped = true;
                    }
                    break;
                case State.Attack:
                    if (character.health > 0)
                    {
                        anim.SetBool("isWalk", false);
                        agent.isStopped = true;
                        anim.SetBool("isAttacking", true);
                        if (Time.time > nextAttackTime)
                        {
                            character.health -= damage;
                            nextAttackTime = Time.time + eachAttackTime;
                        }
                        if (Vector3.Distance(player.position, transform.position) > attackRange)
                        {
                            anim.SetBool("isAttacking", false);
                            curState = State.Chase;
                            //GetComponentInChildren<RightHandZombieAttack>().enabled = false;
                            //GetComponentInChildren<LeftHandZombieAttack>().enabled = false;
                        }
                    }
                    else
                    {
                        anim.SetBool("isAttacking", false);
                    }
                    break;
                case State.GoToLastSighting:
                    agent.destination = lastSighting;
                    if(agent.remainingDistance < 0.5f)
                    {
                        anim.SetBool("isWalking", false);
                        agent.isStopped = true;
                        StartCoroutine(StopAndLook());
                    }
                    break;

            }
        }
    }

    public void FindTarget()
    {
        if (curState == State.Patrol || curState == State.GoToLastSighting)
        {
            Vector3 direction = player.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle <= fieldOfVision * uiManager.fieldOfVisionModifier / 2 && FindPlayer(detectionDistance * uiManager.detectionDistanceModifier))
            {
                if (Vector3.Distance(transform.position, player.position) <= attackRange)
                {
                    curState = State.Attack;
                }
                else
                {
                    curState = State.Chase;
                }
            }
        }
    }

    private bool FindPlayer(float distance)
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * 1.5f, player.position + Vector3.up * uiManager.rayCastPointModifier - transform.position - Vector3.up * 1.5f, Color.red);
        if (Physics.Raycast(transform.position + Vector3.up * 1.5f, player.position + Vector3.up * uiManager.rayCastPointModifier - transform.position - Vector3.up * 1.5f, out hit, distance))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                lastSighting = hit.transform.position;
                return true;
            }
        }
        return false;
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    IEnumerator StopAndLook()
    {
        yield return new WaitForSeconds(3f);
        curState = State.Patrol;
    }
}
