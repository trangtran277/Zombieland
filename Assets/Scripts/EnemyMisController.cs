using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMisController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float speed = 0.25f;
    [SerializeField] float health = 100f;
    [SerializeField] float damageToPlayer = 10f;
    [SerializeField] float detectionDistance = 10f;
    [SerializeField] float fieldOfVision = 240f;
    [SerializeField] float attachRange = 1f;
    [SerializeField] float chaseTime = 4f;

    private Animator anim;
    private bool isAlive = true;
    private bool playerDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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

            //attack when in range
            if(distFromPlayer <= attachRange)
            {
                    
                anim.SetBool("isAttacking", true);                
            }
            else
            {
                anim.SetBool("isAttacking", false);
            }

            if (playerDetected)
            {
                //disregard height differences
                direction.y = 0;

                //rotate to look at player
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);


                //move closer to attack when detected player
                if (direction.magnitude > attachRange)
                {
                    transform.position = Vector3.Lerp(transform.position, player.position, speed * Time.deltaTime);
                    anim.SetBool("isWalking", true);
                }
                else
                {
                    anim.SetBool("isWalking", false);
                }
            }
            else
            {
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
