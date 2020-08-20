using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityScript.Macros;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemeControlPatrolPath : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isAlive = true;
    public Transform player;
    float detectionDistance = 10f;
    bool checkZombieFollow = false;
    [SerializeField] float fieldOfVision = 120f;
    public ThirdPersonCharacter thirdPersonCharacter;
    private float healthofPlayer;
    public bool playerAlive = true;

    public Transform[] navPoints;
    public NavMeshAgent agent;
    public float AIMoveSpeed;

    Animator ani;


    void Start()
    {
        agent.destination = navPoints[Ramdom()].position;

        ani = this.GetComponent<Animator>();
        ani.SetBool("isWalk", true);
        GotoNextPoint();
        agent.speed = AIMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(ani.GetCurrentAnimatorStateInfo(0).IsName("Z_Idle"))
        {
            ani.SetBool("isWalk", true);
            GotoNextPoint();
        }
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Z_Walk"))
        {
            if(agent.remainingDistance<=0.5f)
            {
                GotoNextPoint();
            }
        }
        EnemyControl();
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
    void EnemyControl()
    {
        if (!isAlive)
        {
            isAlive = false;
            ani.SetTrigger("isDead");
            Invoke("DestroyEnemy", 3f);
        }
        else
        {

            // Debug.Log("go here");
            Vector3 direction = player.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);
            float distFromPlayer = Vector3.Distance(player.position, this.transform.position);

            if (distFromPlayer > detectionDistance)
            {
                agent.destination = this.transform.position;
                ani.SetBool("isWalking", false);
                ani.SetBool("isAttacking", false);
                if (checkZombieFollow)
                {
                    DetectionManager.instance.SetChase(false);
                    DetectionManager.instance.isBeingChased = false;
                    checkZombieFollow = false;
                }
            }
            if (angle > fieldOfVision / 2)
            {
                agent.destination = this.transform.position;
                ani.SetBool("isWalking", false);
                ani.SetBool("isAttacking", false);
                if (checkZombieFollow)
                {
                    DetectionManager.instance.SetChase(false);
                    DetectionManager.instance.isBeingChased = false;
                    checkZombieFollow = false;
                }
            }

            if (distFromPlayer <= detectionDistance && FindPlayer(this.transform))
            {
                //
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
                    ani.SetBool("isWalking", true);

                    if (Vector3.Distance(player.position, this.transform.position) <= agent.stoppingDistance)
                    {
                        FaceTarget();
                        ani.SetBool("isWalking", false);
                        ani.SetBool("isAttacking", true);
                    }
                    else
                    {
                        FaceTarget();
                        ani.SetBool("isAttacking", false);
                        ani.SetBool("isWalking", true);
                    }
                }
                else
                {
                    if (ani.GetBool("isWalking"))
                    {
                        agent.destination = this.transform.position;
                        ani.SetBool("isWalking", false);
                        ani.SetBool("isAttacking", false);
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
            ani.SetBool("isWalking", false);
            ani.SetBool("isAttacking", false);
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
}
