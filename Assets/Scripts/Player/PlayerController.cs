using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using TMPro;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private enum CDTimers : int
    {
        attackCD,
        attackDuration,
        numberOfTimers
    }
    [Header("Speed Stats")]

    public float acceleration;
    public float maxSpeed;
    private float currentMaxSpeed;

    [Header("Attack stats")]

    public float attackDelay;
    private float originalAttackDelay;
    public float attackSpeed;
    private float originalAttackSpeed;
    public int damage;
    public int baseDamage;
    public int currentDamage;
    public GameObject weapon;
    public PolygonCollider2D weaponCollider;

    [Header("Child OBJ")]

    public Rigidbody2D rb;
    public Transform centre;
    public Animator anim;
    public TMP_Text attackText;

    [Header("Debug Values")]

    [SerializeField]
    private Vector2 direction;
    [SerializeField]
    private Vector2 LastDirection;
    public Vector2 mousePos { get; private set; }


    private PlayerInputs playerInputs;
    private Camera cam;
    [SerializeField]
    private Timers timers;
    private List<GameObject> enemiesHit = new List<GameObject>();
    private bool hitHeart = false;
    // Start is called before the first frame update
    private void Awake()
    {
        playerInputs = new PlayerInputs();
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 120;
    }
    void Start()
    {
        timers = new Timers((int)CDTimers.numberOfTimers);
        cam = Camera.main;
        currentMaxSpeed = maxSpeed;
        SetWeapon(weapon);
        ResetDamage();
    }

    // Update is called once per frame
    private void OnEnable()
    {
        playerInputs.Enable();
        playerInputs.Player.Move.performed += SetDirection;
        playerInputs.Player.Move.canceled += SetDirection;
        playerInputs.Player.Fire.performed += Attack;

    }

    private void OnDisable()
    {
        playerInputs.Player.Move.performed -= SetDirection;
        playerInputs.Player.Move.canceled -= SetDirection;
        playerInputs.Disable();
    }
    private void SetDirection(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>().normalized;
    }
    // Update is called once per frame
    void Update()
    {
        SetMousePos();
        RotateWeapon();
        StopAttack();
        if (!direction.Equals(Vector2.zero))
            LastDirection = direction;
        /*else if (CurrentState == actionState.moving)
        {
            CurrentState = actionState.idle;
        }
        ExecuteState();
        DashCounter();
        Block();*/
        timers.TickTimer(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (rb.velocity.magnitude <= currentMaxSpeed && !direction.Equals(Vector2.zero))
        {
            if ((rb.velocity + direction * acceleration * rb.mass).magnitude > currentMaxSpeed)
            {
                rb.velocity += direction * acceleration * rb.mass;
                rb.velocity = rb.velocity.normalized * currentMaxSpeed;
            }
            else
            {
                rb.AddForce(direction * acceleration * rb.mass, ForceMode2D.Impulse);
            }
        }
    }

    private void SetMousePos()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (!timers.time[(int)CDTimers.attackCD].Equals(0))
            return;
        if (!GameManager.instance.currentState.Equals(gameState.fighting))
            return;
        weaponCollider.enabled = true;
        timers.time[(int)CDTimers.attackCD] = attackDelay;
        timers.time[(int)CDTimers.attackDuration] = attackSpeed;
        Vector2 dir = (mousePos - (Vector2)transform.position).normalized;
        anim.Play("SwingWeapon");
        //centre.position += (Vector3)dir;
        // play animation and state control in inherited classes

    }

    private void StopAttack()
    {
        if (timers.time[(int)CDTimers.attackDuration].Equals(0) && weaponCollider.enabled)
        {
            weaponCollider.enabled = false;
            enemiesHit.Clear();
            centre.localPosition = Vector3.zero;
        }            
    }

    private void RotateWeapon()
    {
        if (!timers.time[(int)CDTimers.attackDuration].Equals(0))
            return;
        var dir = Input.mousePosition - cam.WorldToScreenPoint(centre.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        centre.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void SetWeapon(GameObject weapon)
    {
        this.weapon = weapon;
        weaponCollider = this.weapon.GetComponent<PolygonCollider2D>();
        anim = GetComponentInChildren<Animator>();
        anim.enabled = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.T_Enemy))
        {
            if (!enemiesHit.Contains(collision.gameObject))
            {
                enemiesHit.Add(collision.gameObject);
                collision.GetComponent<HealthComp>().TakeDamage(currentDamage);
            }
        }
        if (collision.gameObject.CompareTag(Tags.T_Heart) && !hitHeart)
        {
            hitHeart = true;
            GameManager.instance.lowerEnemyHealth(currentDamage);
            collision.GetComponentInChildren<TMP_Text>().text = GameManager.instance.enemyHealth.ToString();
            Invoke("RemoveHeart", 2);
        }
    }
    public void RemoveHeart()
    {
        hitHeart = false;
        GameObject heart = GameObject.FindWithTag(Tags.T_Heart);
        Vector3 currentPos = Vector2.up * 2.5f;

        Vector3 targetPos = currentPos;
        targetPos.y = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + 1;
        heart.transform.DOMove(targetPos, 1.0f).SetEase(DG.Tweening.Ease.InOutQuad);
        //Destroy();
        EnemyManager.instance.SpawnShop();
    }
    public void ResetDamage()
    {
        currentDamage = damage + baseDamage;
        attackText.text = currentDamage.ToString();
    }

    public void AddDamage(int amount)
    {
        currentDamage += amount;
        attackText.text = currentDamage.ToString();
    }

    public void ResetStats()
    {
        if (BuffManager.instance.isSwitched)
        {
            BuffManager.instance.isSwitched = false;
            GameManager.instance.playerHealth.currentHealth = currentDamage;
        }
        ResetDamage();
        ResetSpeed();
        ResetMoveSpeed();
    }

    [ContextMenu("increase Attack Speed")]
    public void IncreaseAttackSpeed()
    {
        originalAttackDelay = attackDelay;
        originalAttackSpeed = attackSpeed;
        anim.speed = (1.5f);
        attackDelay *= 0.66666666666f;
        attackSpeed *= 0.66666666666f;
    }

    public void ResetSpeed()
    {
        if (originalAttackDelay == 0)
            return;
        attackDelay = originalAttackDelay;
        attackSpeed = originalAttackSpeed;
        anim.speed = 1;
        originalAttackDelay = 0;
        originalAttackSpeed = 0;
    }

    public void IncreaseSpeed(float speed)
    {
        currentMaxSpeed += speed;
    }

    public void ResetMoveSpeed()
    {
        currentMaxSpeed = maxSpeed;
    }

    public void ChangeWeapon(Weapons weaponSO)
    {
        DestroyImmediate(weapon);
        GameObject newWep = Instantiate(weaponSO.weaponObj, centre.transform);
        newWep.transform.localPosition = Vector3.up * weaponSO.Ydist;
        SetWeapon(newWep);
        currentDamage = currentDamage - damage + weaponSO.damage;
        damage = weaponSO.damage;
        bool increaseSpeed = originalAttackDelay != 0;
        if (increaseSpeed)
        {
            ResetSpeed();
            attackDelay = weaponSO.attackDelay;
            attackSpeed = weaponSO.attackSpeed;
            IncreaseAttackSpeed();
        }
        else
        {
            attackDelay = weaponSO.attackDelay;
            attackSpeed = weaponSO.attackSpeed;
        }
    }
}
