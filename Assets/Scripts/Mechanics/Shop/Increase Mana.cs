using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IncreaseMana : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isOverlapping;
    private PlayerInputs inputs;
    public int manaIncrease;
    public int tokenCost;
    private void Awake()
    {
        inputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Interact.performed += BuyMaxMana;
    }

    private void OnDisable()
    {

        inputs.Player.Interact.performed -= BuyMaxMana;
        inputs.Disable();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyMaxMana(InputAction.CallbackContext callbackContext)
    {
        if (!isOverlapping)
            return;
        if(GameManager.instance.UseToken(tokenCost))
        {
            GameManager.instance.maxMana += manaIncrease;
            GameManager.instance.SetMana();
            GameUI.instance.ManaText();
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
