using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timers
{
    // Start is called before the first frame update
    public List<float> time = new List<float>();

    // Update is called once per frame
    public Timers (int amountOfTimers)
    {
        for (int i = 0; i < amountOfTimers; i++)
        {
            time.Add(0f);
        }
    }

    public void TickTimer(float deltaTime)
    {
        for (int i = 0; i < time.Count; i++)
        {
            if (time[i] > 0)
                time[i] -= deltaTime;
            else if (time[i] < 0)
            {
                //Debug.Log(timers[i] + " " + i);
                time[i] = 0;
            }
        }
    }
}
