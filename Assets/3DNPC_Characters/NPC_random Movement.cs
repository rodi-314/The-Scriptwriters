using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_random : MonoBehaviour
{
    public float speed = 2.0f;
    public float maxSpeed = 3.0f;  // Maximum speed limit
    public float changeDirectionTime = 3.0f;  // Time to change direction
    public float waitTime = 2.0f;  // Time to wait at each point
    public float maxDistance = 5.0f;  // Maximum distance from the original position
    public float collisionAngleChange = 30.0f;  // Maximum angle change on collision
    private Vector2 originalPos;
    private Vector2 targetPos;
    private Animator animator;
    private float changeDirectionTimer;
    private float waitTimer;
    private bool waiting = false;

    private Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalPos = rigidbody2D.position;
        targetPos = GetRandomTargetPos();
        changeDirectionTimer = changeDirectionTime;
        waitTimer = waitTime;
    }

    void Update()
    {
        if (waiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                waiting = false;
                targetPos = GetRandomTargetPos();
                waitTimer = waitTime;
            }
        }
        else
        {
            changeDirectionTimer -= Time.deltaTime;
            if (changeDirectionTimer <= 0)
            {
                changeDirectionTimer = changeDirectionTime;
                targetPos = GetRandomTargetPos();
            }
        }
    }

    void FixedUpdate()
    {
        if (!waiting)
        {
            Vector2 position = Vector2.MoveTowards(rigidbody2D.position, targetPos, Mathf.Min(speed, maxSpeed) * Time.deltaTime);
            Vector2 direction = (targetPos - rigidbody2D.position).normalized;

            animator.SetFloat("Move_X", direction.x);
            animator.SetFloat("Move_Y", direction.y);

            rigidbody2D.MovePosition(position);

            if (Vector2.Distance(rigidbody2D.position, targetPos) < 0.1f)
            {
                waiting = true;
                animator.SetFloat("Move_X", 0);
                animator.SetFloat("Move_Y", 0);
            }
        }
    }

    Vector2 GetRandomTargetPos()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(0, maxDistance);
        Vector2 randomTargetPos = originalPos + randomDirection * randomDistance;

        return randomTargetPos;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        MC_Controller player = other.gameObject.GetComponent<MC_Controller>();

        if (player != null)
        {
            speed = 0;
            // Freeze position to prevent NPC from being pushed
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            // Change direction by a random angle less than 30 degrees on collision
            float angle = Random.Range(-collisionAngleChange, collisionAngleChange);
            Vector2 currentDirection = (targetPos - rigidbody2D.position).normalized;
            Vector2 newDirection = Quaternion.Euler(0, 0, angle) * currentDirection;
            targetPos = rigidbody2D.position + newDirection * maxDistance;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        MC_Controller player = other.gameObject.GetComponent<MC_Controller>();

        if (player != null)
        {
            speed = maxSpeed; // Restore the original speed
            // Unfreeze position but keep rotation frozen
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
