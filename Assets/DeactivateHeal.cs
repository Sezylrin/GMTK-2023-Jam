using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateHeal : MonoBehaviour
{

    private void OnEnable()
    {
        Invoke("StopHeal", 1.5f);
    }


    public void StopHeal()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
