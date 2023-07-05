using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class MainMenuShake : MonoBehaviour
{
    public ShakeData shakeData;

    private void Start()
    {
        CameraShakerHandler.Shake(shakeData);
    }
    
}
