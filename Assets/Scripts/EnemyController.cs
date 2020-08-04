using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    private Animator anim;
    public float speed = 1;
    public float detectionDistance = 10f;
    public float fieldOfVision = 120f;
    public float alwaysDectectCircle = 2f;
    public float attachRange = 0.9f;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);
        float distFromPlayer = Vector3.Distance(player.position, this.transform.position);

        if (distFromPlayer < detectionDistance && (angle < fieldOfVision || distFromPlayer < alwaysDectectCircle))
        {
            direction.y = 0;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

            if(distFromPlayer <= attachRange)
            {
                anim.SetBool("isAttacking", true);                
            }
            else
            {
            anim.SetBool("isAttacking", false);
            }

            if (direction.magnitude > attachRange)
            {
                //this.transform.Translate(0, 0, 0.5f);
                transform.position = Vector3.Lerp(transform.position, player.position, speed * Time.deltaTime);
                anim.SetBool("isWalking", true);
            }
            else
            {
                //anim.SetBool("isAttacking", true);
                anim.SetBool("isWalking", false);
            }
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }
}
