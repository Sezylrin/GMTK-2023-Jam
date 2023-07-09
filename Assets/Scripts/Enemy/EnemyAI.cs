using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState : int
    {
        idle,
        chasing,
        readyAttack,
        attacking
    }

    public enum EnemyTimers : int
    {
        attackDelay,
        attackDuration,
        prepAttack,
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
    public GameObject weapon;
    private PolygonCollider2D weaponCollider;
    public GameObject weaponCentrePoint;
    public float attackPrep;
    [Header("Child OBJ")]

    public Rigidbody2D rb;
    public Animator anim;
    public TMP_Text attackText;
    public List<GameObject> particles = new List<GameObject>();

    [Header("Debug")]
    [SerializeField]
    private EnemyState currentState;
    [SerializeField]
    private Timers timers;

    void Start()
    {
        currentState = EnemyState.idle;
        currentMaxSpeed = maxSpeed;
        timers = new Timers((int)EnemyTimers.numberOfCD);
        target = GameObject.FindWithTag(Tags.T_Player).transform;


        SetWeapon(weapon);
        SetDamageText();
    }
    public void SetDamage(int amount)
    {
        damage = amount;
        SetDamageText();
    }
    void Update()
    {
        Attack();
        StopAttacking();
        AimAttack();
        timers.TickTimer(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        if (currentState == EnemyState.idle)
            return;
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
        if (currentState == EnemyState.idle)
            return;
        if (!currentState.Equals(EnemyState.readyAttack))
            return;
        if (timers.time[(int)EnemyTimers.attackDelay].Equals(0))
        {
            rb.bodyType = RigidbodyType2D.Static;
            currentState = EnemyState.attacking;
            //attack by playing animation etc.
            Invoke("DelayAttack", attackPrep);
            timers.time[(int)EnemyTimers.attackDelay] = attackDelay + attackPrep;

            timers.time[(int)EnemyTimers.attackDuration] = attackDuration + attackPrep;
        }
        else
        {
            currentState = EnemyState.chasing;
        }
    }
    private void DelayAttack()
    {
        weaponCollider.enabled = true;
        anim.Play("SwingWeapon");
    }
    private void AimAttack()
    {
        if (currentState == EnemyState.idle)
            return;
        if (currentState != EnemyState.chasing)
            return;
        Vector2 dir = target.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        weaponCentrePoint.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
    }
    private void StopAttacking()
    {
        if (currentState == EnemyState.idle)
            return;
        if (currentState == EnemyState.attacking && timers.time[(int)EnemyTimers.attackDuration].Equals(0))
        {
            weaponCollider.enabled = false;
            weaponCentrePoint.transform.rotation = Quaternion.Euler(Vector3.zero);
            currentState = EnemyState.chasing;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void SetWeapon(GameObject weapon)
    {
        this.weapon = weapon;
        weaponCollider = this.weapon.GetComponent<PolygonCollider2D>();
        anim = GetComponentInChildren<Animator>();
        anim.enabled = true;
        
    }

    public void IncreaseSpeed()
    {
        attackDelay *= 0.6666666f;
        attackDuration *= 0.6666666f;
        anim.speed = 1.5f;
    }
    public void ChangeWeapon(Weapons weaponSO)
    {
        DestroyImmediate(weapon);
        GameObject newWep = Instantiate(weaponSO.weaponObj, weaponCentrePoint.transform);
        newWep.transform.localPosition = Vector3.up * weaponSO.Ydist;
        SetWeapon(newWep);
        attackDelay = weaponSO.attackDelay;
        attackDuration = weaponSO.attackSpeed;
        attackDist = weaponSO.attackDist;
        attackPrep = weaponSO.attackPrepTime;        
    }

    public void SetDamageText()
    {
        attackText.text = damage.ToString();
    }
    public void IncreaseMoveSpeed(float increaseSpeed)
    {
        currentMaxSpeed += increaseSpeed;
    }

    public void SetChase()
    {
        currentState = EnemyState.chasing;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.T_Player))
        {
            GameManager.instance.playerHealth.TakeDamage(damage);
        }
    }
}
