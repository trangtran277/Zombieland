using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.Audio;

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
    public float soundDetectionDistance = 4f;
    public float fieldOfVision = 120f;
    public float stopChasingDistance = 17f;
    public float attackRange = 1f;
    public float eachAttackTime = 2f;
    public float damage = 20f;
    public float speedWhileChase;
    public float speedWhilePatrol;
    public bool isAlive = true;
    public Transform[] PatrolPoints;
    public LayerMask layerMask;
    public int curIndex = -1;

    DetectionManager detectionManager;
    UiManagerController uiManager;
    ThirdPersonCharacter character;

    private Animator anim;
    private NavMeshAgent agent;
    [SerializeField]private State curState;
    private float nextAttackTime = 0;
    private Vector3 lastSighting;
    private Animator playerAnimator;
    private AudioSource[] audios;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        playerAnimator = player.GetComponent<Animator>();
        detectionManager = DetectionManager.instance;
        uiManager = UiManagerController.instance;
        character = player.GetComponent<ThirdPersonCharacter>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speedWhilePatrol;
        if (PatrolPoints.Length > 0)
        {
            curIndex = 0;
        }
        curState = State.Patrol;
        nextAttackTime = Time.time;
        audios = GetComponents<AudioSource>();
        if (audios.Length > 0)
            audios[0].Play();

    }
    void Update()
    {
        if (!isAlive)
        {
            if (audios.Length > 0)
            {
                audios[0].Stop();
                audios[1].Stop();
                audios[2].Play();
            }
            agent.isStopped = true;
            anim.SetTrigger("isDead");
            detectionManager.ActivateDetectionPointer(false, null, null, this);
            foreach (Collider c in GetComponents<Collider>())
                c.enabled = false;
            Invoke("DestroyEnemy", 2f);
            enabled = false;
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
                        if (agent.remainingDistance <= 0.5f)
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
                        if (Vector3.Distance(transform.position, player.position) <= attackRange)
                        {
                            anim.SetBool("isWalking", false);
                            agent.isStopped = true;
                            curState = State.Attack;
                        }
                        if (!FindPlayer(stopChasingDistance))
                        {
                            curState = State.GoToLastSighting;
                            detectionManager.ActivateDetectionPointer(false, null, null, this);
                            if (audios.Length > 0)
                            {
                                audios[1].Stop();
                                audios[0].Play();
                            }
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
                    if(agent.remainingDistance <= 3f)
                    {
                        agent.isStopped = true;
                        anim.SetBool("isWalking", false);
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
                if (audios.Length > 0)
                {
                    audios[0].Stop();
                    audios[1].Play();
                }
            }
            else if(Vector3.Distance(player.position, transform.position) <= soundDetectionDistance && playerAnimator.GetFloat("Forward") >= 0.8 && !playerAnimator.GetBool("crouch"))
            {
                if (Vector3.Distance(transform.position, player.position) <= attackRange)
                {
                    curState = State.Attack;
                }
                else
                {
                    curState = State.Chase;
                }
                lastSighting = player.position;
                if (audios.Length > 0)
                {
                    audios[0].Stop();
                    audios[1].Play();
                }
            }
        }
    }

    private bool FindPlayer(float distance)
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * 1.5f, player.position + Vector3.up * uiManager.rayCastPointModifier - transform.position - Vector3.up * 1.5f, Color.red);
        if (Physics.Raycast(transform.position + Vector3.up * 1.5f, player.position + Vector3.up * uiManager.rayCastPointModifier - transform.position - Vector3.up * 1.5f, out hit, distance, layerMask))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                lastSighting = hit.transform.position;
                detectionManager.ActivateDetectionPointer(true, transform, hit.transform, this);
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
