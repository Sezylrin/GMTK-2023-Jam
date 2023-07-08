using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState : int
    {
        chasing,
        readyAttack,
        attacking
    }

    public enum EnemyTimers : int
    {
        attackDelay,
        attackDuration,
        numberOfCD
    }
    [Header("Core")]

    public Transform target;
    public float attackDist;
    [Header("Speed Stats")]

    public float acceleration;
    public float maxSpeed;
    private float currentMaxSpeed;

    [Header("Attack Stats")]

    public float attackDelay;
    public float attackDuration;
    public int damage;
    private int currentDamage;
    public GameObject weapon;
    private PolygonCollider2D weaponCollider;

    [Header("Child OBJ")]

    public Rigidbody2D rb;
    public Animator anim;

    [Header("Debug")]
    [SerializeField]
    private EnemyState currentState;
    [SerializeField]
    private Timers timers;

    void Start()
    {
        currentState = EnemyState.chasing;
        currentMaxSpeed = maxSpeed;
        timers = new Timers((int)EnemyTimers.numberOfCD);
        target = GameObject.FindWithTag(Tags.T_Player).transform;
    }
    void Update()
    {
        Attack();
        StopAttacking();
        timers.TickTimer(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        if (Vector2.Distance(transform.position,target.position) > attackDist && currentState.Equals(EnemyState.chasing))
        {
            if (rb.velocity.magnitude <= currentMaxSpeed)
            {
                if ((rb.velocity + (Vector2)(target.position - transform.position) * acceleration * rb.mass).magnitude > currentMaxSpeed)
                {
                    rb.velocity += (Vector2)(target.position - transform.position) * acceleration * rb.mass;
                    rb.velocity = rb.velocity.normalized * currentMaxSpeed;
                }
                else
                {
                    rb.AddForce((Vector2)(target.position - transform.position) * acceleration * rb.mass, ForceMode2D.Impulse);
                }
            }
        }
        else if (timers.time[(int)EnemyTimers.attackDelay].Equals(0))
        {
            currentState = EnemyState.readyAttack;
        }
    }

    private void Attack()
    {
        if (!currentState.Equals(EnemyState.readyAttack))
            return;
        if (timers.time[(int)EnemyTimers.attackDelay].Equals(0))
        {
            rb.bodyType = RigidbodyType2D.Static;
            currentState = EnemyState.attacking;
            //attack by playing animation etc.
            anim.Play("SwingSword");
            timers.time[(int)EnemyTimers.attackDuration] = attackDuration;
            timers.time[(int)EnemyTimers.attackDelay] = attackDelay;
        }
        else
        {
            currentState = EnemyState.chasing;
        }
    }

    private void StopAttacking()
    {
        if (currentState == EnemyState.attacking && timers.time[(int)EnemyTimers.attackDuration].Equals(0))
        {
            currentState = EnemyState.chasing;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
