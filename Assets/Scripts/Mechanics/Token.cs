using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public List<AudioClip> clips;
    public SpriteRenderer render;
    public CircleCollider2D col;
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
            audioSource.clip = clips[Random.Range(0, 2)];
            audioSource.Play();
            GameManager.instance.tokens++;
            GameUI.instance.TokenText();
            render.enabled = false;
            col.enabled = false;
            Invoke("DestroySelf", 0.3f);
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
