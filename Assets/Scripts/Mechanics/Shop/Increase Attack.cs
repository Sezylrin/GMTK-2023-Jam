using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IncreaseAttack : MonoBehaviour
{
    private bool isOverlapping;
    private PlayerInputs inputs;
    public int increaseAttack;
    public int tokenCost;
    public AudioSource audioSource;
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
        inputs.Player.Interact.performed += BuyDamage;
    }

    private void OnDisable()
    {

        inputs.Player.Interact.performed -= BuyDamage;
        inputs.Disable();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void BuyDamage(InputAction.CallbackContext callbackContext)
    {
        if (!isOverlapping)
            return;
        if (GameManager.instance.UseToken(tokenCost))
        {
            PlayerController temp = GameManager.instance.player.GetComponent<PlayerController>();
            temp.baseDamage += increaseAttack;
            temp.ResetDamage();
            audioSource.Play();
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
