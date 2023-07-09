using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IncreaseHealth : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isOverlapping;
    private PlayerInputs inputs;
    public int healthIncrease;
    public int tokenCost;
    private void Awake()
    {

        inputs = new PlayerInputs();
    }
    void Start()
    {
    }
    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Interact.performed += BuyMaxHealth;
    }

    private void OnDisable()
    {

        inputs.Player.Interact.performed -= BuyMaxHealth;
        inputs.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyMaxHealth(InputAction.CallbackContext callbackContext)
    {
        if (!isOverlapping)
            return;
        if (GameManager.instance.UseToken(tokenCost))
        {
            HealthComp temp = GameManager.instance.player.GetComponent<HealthComp>();
            temp.maxHealth += healthIncrease;
            temp.heal(healthIncrease);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.T_Player))
            isOverlapping = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.T_Player))
            isOverlapping = false;
    }
}
