using HutongGames.PlayMaker.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyControlPatrolPath : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Transform player;
    [SerializeField] float detectionDistance = 10f;
    [SerializeField] float fieldOfVision = 120f;
    public ThirdPersonCharacter thirdPersonCharacter;
    public bool isAlive = true;

    private Animator anim;
    private NavMeshAgent agent;
    private float healthofPlayer;
    public bool playerAlive = true;

    public Transform[] PatrolPoints;
    bool checkZombieFollow = false;
    int index = 0;


    // Start is called before the first frame update
    void Start()
    {
        //healthofPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>().health;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (thirdPersonCharacter == null)
        {
            thirdPersonCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>();
        }
        anim = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        agent.speed = 0.5f;
        index = Random.Range(0, PatrolPoints.Length);
        agent.destination = PatrolPoints[index].position;
        anim.SetBool("isWalk", true);
    }

    // Update is called once per frame
    void Update()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //Debug.Log(healthofPlayer);
        EnemyControl(); 
    }
    void EnemyControl()
    {
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
            float distFromPlayer = Vector3.Distance(player.position, this.transform.position);

            if(agent.remainingDistance <= 1f)
            {
                GotoNextPoint();
            }
            if (distFromPlayer > detectionDistance)
            {
                agent.destination = this.transform.position;
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", false);
                if (checkZombieFollow)
                {
                    DetectionManager.instance.SetChase(false);
                    DetectionManager.instance.isBeingChased = false;
                    checkZombieFollow = false;
                }
                agent.destination = PatrolPoints[index].position;
            }
            if (angle > fieldOfVision / 2)
            {
                agent.destination = this.transform.position;
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", false);
                if (checkZombieFollow)
                {
                    DetectionManager.instance.SetChase(false);
                    DetectionManager.instance.isBeingChased = false;
                    checkZombieFollow = false;
                }
                agent.destination = PatrolPoints[index].position;
            }

            if (distFromPlayer <= detectionDistance && FindPlayer(this.transform))
            {
                //
                anim.SetBool("isWalk", false);
                if (angle <= fieldOfVision / 2)
                {
                    agent.destination = player.position;
                    checkZombieFollow = true;
                    if (!DetectionManager.instance.isBeingChased)
                    {
                        DetectionManager.instance.isBeingChased = true;
                        DetectionManager.instance.SetChase(true);
                        DetectionManager.instance.isNearDetected = false;
                        DetectionManager.instance.SetDitection(false);
                    }
                    FaceTarget();
                    anim.SetBool("isWalking", true);

                    if (Vector3.Distance(player.position, this.transform.position) <= agent.stoppingDistance)
                    {
                        FaceTarget();
                        anim.SetBool("isWalking", false);
                        anim.SetBool("isAttacking", true);
                    }
                    else
                    {
                        FaceTarget();
                        anim.SetBool("isAttacking", false);
                        anim.SetBool("isWalking", true);
                    }
                }
                else
                {
                    if (anim.GetBool("isWalking"))
                    {
                        agent.destination = this.transform.position;
                        anim.SetBool("isWalking", false);
                        anim.SetBool("isAttacking", false);
                        agent.destination = PatrolPoints[index].position;
                    }
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
            healthofPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>().health;
        if (healthofPlayer <= 0)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);
            playerAlive = false;
        }
    }
    void FaceTarget()
    {
        Vector3 direction = (player.position - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    private bool FindPlayer(Transform target)
    {
        RaycastHit hit;
        for (float x = -1f; x <= 1f; x += 0.2f)
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
    void GotoNextPoint()
    {
        anim.SetBool("isWalk", true);
        index = Random.Range(0, PatrolPoints.Length);
        agent.destination = PatrolPoints[index].position;
    }
}
