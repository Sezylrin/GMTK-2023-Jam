using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {    
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentState.Equals(gameState.shop))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.T_Player))
        {
            GameManager.instance.tokens++;
            GameUI.instance.TokenText();
            Destroy(gameObject);
        }
    }
}
