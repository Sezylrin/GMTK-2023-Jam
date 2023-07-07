using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public float attackSpeed;
    public float damage;
    public GameObject weapon;
    private PolygonCollider2D weaponCollider;

    [Header("Child OBJ")]

    public Rigidbody2D rb;
    public Transform centre;

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
        weaponCollider.enabled = true;
        timers.time[(int)CDTimers.attackCD] = attackDelay;
        timers.time[(int)CDTimers.attackDuration] = attackSpeed;
        Vector2 dir = (mousePos - (Vector2)transform.position).normalized;
        centre.position += (Vector3)dir;
        // play animation and state control in inherited classes

    }

    private void StopAttack()
    {
        if (timers.time[(int)CDTimers.attackDuration].Equals(0) && weaponCollider.enabled)
        {
            weaponCollider.enabled = false;
            centre.localPosition = Vector3.zero;
        }
            
    }

    private void RotateWeapon()
    {
        var dir = Input.mousePosition - cam.WorldToScreenPoint(centre.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        centre.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void SetWeapon(GameObject weapon)
    {
        this.weapon = weapon;
        weaponCollider = this.weapon.GetComponent<PolygonCollider2D>();
    }
}
