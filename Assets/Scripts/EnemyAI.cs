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
    private Quaternion lastRotation;
    private Animator playerAnimator;
    private AudioSource[] audios;
    private bool isStopAndLooking = false;

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
            StartCoroutine(StartAudio());

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
                    if (!audios[0].isPlaying)
                    {
                        audios[0].Play();
                        audios[1].Stop();
                    }
                    anim.SetBool("isWalking", false);
                    if (curIndex >= 0)
                    {
                        anim.SetBool("isWalk", true);
                        agent.isStopped = false;
                        agent.destination = PatrolPoints[curIndex].position;
                        agent.speed = speedWhilePatrol;
                        if (Vector3.Distance(transform.position, PatrolPoints[curIndex].position) <= 1f)
                        {
                            curIndex += 1;
                            if (curIndex > PatrolPoints.Length - 1)
                                curIndex = 0;
                            agent.destination = PatrolPoints[curIndex].position;
                        }
                    }
                    else
                    {
                        agent.destination = transform.position;
                    }
                    //FindTarget();
                    break;
                case State.Chase:
                    if (character.health > 0)
                    {
                        if (!audios[1].isPlaying)
                        {
                            audios[1].Play();
                            audios[0].Stop();
                        }
                        isStopAndLooking = false;
                        anim.SetBool("isWalk", false);
                        anim.SetBool("isWalking", true);
                        agent.isStopped = false;
                        agent.destination = lastSighting;
                        agent.speed = speedWhileChase;
                        detectionManager.ActivateDetectionPointer(true, transform, player, this);
                        if (Vector3.Distance(transform.position, player.position) <= attackRange)
                        {
                            anim.SetBool("isWalking", false);
                            agent.isStopped = true;
                            curState = State.Attack;
                        }
                        if (!FindPlayer(stopChasingDistance))
                        {
                            curState = State.GoToLastSighting;
                            //detectionManager.ActivateDetectionPointer(false, null, null, this);
                            /*if (audios.Length > 0)
                            {
                                audios[1].Stop();
                                audios[0].Play();
                            }*/
                        }
                    }
                    else
                    {
                        anim.SetBool("isWalking", false);
                        agent.isStopped = true;
                        if (audios.Length > 0)
                        {
                            audios[1].Stop();
                            audios[0].Play();
                            detectionManager.ActivateDetectionPointer(false, null, null, this);
                        }
                    }
                    break;
                case State.Attack:
                    if (character.health > 0)
                    {
                        isStopAndLooking = false;
                        FaceTarget();
                        if (!audios[1].isPlaying)
                        {
                            audios[1].Play();
                            audios[0].Stop();
                        }
                        anim.SetBool("isWalk", false);
                        agent.isStopped = true;
                        anim.SetBool("isAttacking", true);
                        detectionManager.ActivateDetectionPointer(true, transform, player, this);
                        if (Time.time > nextAttackTime)
                        {
                            character.health -= damage;
                            nextAttackTime = Time.time + eachAttackTime;
                        }
                        if (Vector3.Distance(player.position, transform.position) > attackRange)
                        {
                            anim.SetBool("isAttacking", false);
                            curState = State.Chase;
                        }
                    }
                    else
                    {
                        anim.SetBool("isAttacking", false);
                        anim.SetBool("isWalking", false);
                        if (audios.Length > 0)
                        {
                            audios[1].Stop();
                            audios[0].Play();
                            detectionManager.ActivateDetectionPointer(false, null, null, this);
                        }
                    }
                    break;
                case State.GoToLastSighting:
                    if (!audios[1].isPlaying)
                    {
                        audios[1].Play();
                        audios[0].Stop();
                    }
                    agent.destination = lastSighting;
                    if(isStopAndLooking)
                        detectionManager.ActivateDetectionPointer(false, null, null, this);
                    else
                        detectionManager.ActivateDetectionPointer(true, transform, player, this);
                    if (agent.remainingDistance <= 2f || Vector3.Distance(transform.position, lastSighting) <= 2f)
                    {
                        if(!isStopAndLooking)
                            StartCoroutine(StopAndLook());
                    }
                    break;

            }
        }
    }

    public void FindTarget()
    {
        if (curState == State.Patrol || curState == State.GoToLastSighting || curState == State.Chase)
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
            else if (Mathf.Abs(player.position.y - transform.position.y) <= 1 && Vector3.Distance(player.position, transform.position) <= soundDetectionDistance && playerAnimator.GetFloat("Forward") >= 0.8 && !playerAnimator.GetBool("crouch"))
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
                lastRotation = player.rotation;
                
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
                lastRotation = hit.transform.rotation;
                //detectionManager.ActivateDetectionPointer(true, transform, hit.transform, this);
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
        isStopAndLooking = true;
        agent.destination = transform.position;
        anim.SetBool("isWalking", false);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lastRotation, 5f);
        //detectionManager.ActivateDetectionPointer(false, null, null, this);
        /*if (audios.Length > 0)
        {
            audios[1].Stop();
            audios[0].Play();
        }*/
        yield return new WaitForSeconds(3f);
        if(curState == State.GoToLastSighting)
        {
            curState = State.Patrol;
        }
        isStopAndLooking = false;

    }

    IEnumerator StartAudio()
    {
        float delayTime = Random.Range(0, 5);
        yield return new WaitForSeconds(delayTime);
        audios[0].Play();
    }

    void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
