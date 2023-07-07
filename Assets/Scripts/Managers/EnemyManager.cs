using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> enemies = new List<GameObject>();
    public float separationDist;
    public float separationForce;
    void Start()
    {
        GameObject[] allenemy = GameObject.FindGameObjectsWithTag(Tags.T_Enemy);
        foreach (GameObject obj in allenemy)
        {
            enemies.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SeparateEnemy();
    }

    private void SeparateEnemy()
    {
        foreach (GameObject enemy in enemies)
        {
            foreach (GameObject other in enemies)
            {
                if (other != enemy && Vector3.Distance(enemy.transform.position,other.transform.position) < separationDist)
                {
                    enemy.GetComponent<Rigidbody2D>().AddForce((enemy.transform.position - other.transform.position).normalized * separationForce);
                    other.GetComponent<Rigidbody2D>().AddForce((other.transform.position - enemy.transform.position).normalized * separationForce);
                }
            }
        }
    }
}
